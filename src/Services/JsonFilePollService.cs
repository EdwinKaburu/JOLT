﻿using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Text.Json;
using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Hosting;
using System;

namespace ContosoCrafts.WebSite.Services
{
    /// <summary>
    /// Services and defensive programming for poll related methods 
    /// </summary>
    public class JsonFilePollService
    {

        /// <summary>
        /// WebHostEnvironment knows where the  data file is stored at
        /// </summary>
        public IWebHostEnvironment WebHostEnvironment { get; }


        /// <summary>
        /// Constructor for JsonFilePollService
        /// </summary>
        /// <param name="webHostEnvironment"></param>
        public JsonFilePollService(IWebHostEnvironment webHostEnvironment)
        {
            WebHostEnvironment = webHostEnvironment;
        }

        /// <summary>
        /// specify file path to retrieve from
        /// </summary>
        private string JsonFileName
        {
            //get 
            get
            {
                //return file path 
                return Path.Combine(WebHostEnvironment.WebRootPath,
              "data", "polls.json");
            }
        }

        /// <summary>
        /// De-serialize a JSON of Polls to List
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PollModel> GetPolls()
        {
            //create file reader for JSON file 
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
                // De-serialize JSON to List
                return JsonSerializer.Deserialize<PollModel[]>
                      (jsonFileReader.ReadToEnd(),
                      new JsonSerializerOptions
                      {
                          //make case insensitive
                          PropertyNameCaseInsensitive = true
                      });
            }
        }

        /// <summary>
        /// Save All poll data to storage
        /// </summary>
        /// <param name="polls"></param>
        private void SavePollData(IEnumerable<PollModel> polls)
        {
            using (var outputStream = File.Create(JsonFileName))
            {
                // Serialize Collection to JSON
                JsonSerializer.Serialize<IEnumerable<PollModel>>(
                    new Utf8JsonWriter(outputStream, new JsonWriterOptions
                    {
                        //skip validation 
                        SkipValidation = true,

                        //make indented 
                        Indented = true
                    }),
                    polls
                );
            }
        }

        /// <summary>
        /// Serialize a Opinion Item Collection to JSON
        /// </summary>
        /// <param name="opinions"></param>
        /// <returns></returns>
        public string GetOpinionsJsons(IEnumerable<OpinionItem> opinions)
        {
            // Return JSON Data
            return JsonSerializer.Serialize<IEnumerable<OpinionItem>>(opinions);
        }

        /// <summary>
        /// Return a PollModel based on ID
        /// </summary>
        /// <param name="pollID"></param>
        /// <returns></returns>
        public PollModel GetPoll(int pollID)
        {
            // Get list of Polls
            List<PollModel> polls = GetPolls().ToList();

            // Return Poll found by Poll ID, returns null if ID not found
            return polls.Find(x => x.PollID == pollID);
        }

        /// <summary>
        /// Return a PollModel based on Title
        /// </summary>
        /// <param name="inputTitle"></param>
        /// <returns></returns>
        public PollModel GetPoll(string inputTitle)
        {
            // Get list of Polls
            List<PollModel> polls = GetPolls().ToList();

            // Return Poll found by Poll Title
            return polls.Find(x => x.Title == inputTitle);
        }

        /// <summary>
        /// Checks And Return True if Poll Exist by Title, False Otherwise 
        /// </summary>
        /// <param name="inputTitle"></param>
        /// <returns></returns>
        public bool PollExist(string inputTitle)
        {
            // Fetch A Given Poll
            if (GetPoll(inputTitle) == null)
            {
                // return false for Poll Don't Exists
                return false;
            }

            // return True , for Poll Exists
            return true;
        }
        
        /// <summary>
        /// Get Total Numbers of Votes Of all Opinions In A Poll
        /// </summary>
        /// <param name="poll"></param>
        /// <returns></returns>
        public int GetTotalVotes(PollModel poll)
        {
            // votes counter Attribute
            int votesCounter = 0;

            // Check to see whether Poll is Null
            if(poll == null)
            {
                // Return votesCounter
                return votesCounter;
            }

            // Iterate through Loop
            foreach(OpinionItem items in poll.OpinionItems)
            {
                // Sum Up the Votes in a Poll
                votesCounter += items.NumCounts;
            }

            // Return Sum
            return votesCounter;
        }

        /// <summary>
        /// Checks And Return True if Poll Exist by ID, False Otherwise 
        /// </summary>
        /// <param name="inputID"></param>
        /// <returns></returns>
        public bool PollExist(int inputID)
        {
            // Fetch A Given Poll
            if (GetPoll(inputID) == null)
            {
                // return false for Poll Don't Exists
                return false;
            }

            // return True , for Poll Exists
            return true;
        }

        /// <summary>
        /// Get OpinionItem from a Poll Model based on Name
        /// </summary>
        /// <param name="name"></param>
        /// <param name="pollModel"></param>
        /// <returns></returns>
        public OpinionItem GetOpinion(string name, PollModel pollModel)
        {
            // Check if Poll Model is Null 
            if(pollModel == null)
            {
                return null;
            }

            // Check if Name of Opinion was provided
            if (name == null)
            {
                // No Name was provided
                return null;
            }

            // Fetch Opinion that matches a specific name
            OpinionItem opinion = pollModel.OpinionItems.ToList().Find(x => x.OpinionName.Equals(name));

            // return the opinion
            return opinion;
        }

        /// <summary>
        /// Update Vote Of Opinion 
        /// </summary>
        /// <param name="pollID"></param>
        /// <param name="opinionTitle"></param>
        /// <returns></returns>
        public bool UpdateOpinionVote(int pollID, string opinionTitle)
        {
            
            // Get List of Polls from Data-Set
            List<PollModel> getPolls = GetPolls().ToList();

            // Get Poll Model based on Poll ID
            PollModel pollModel = getPolls.Find(x => x.PollID.Equals(pollID));

            // Return null if Poll doesn't exist
            if(pollModel == null)
            {
                return false;
            }

            // Get Opinion Item based on Title and Poll Model
            OpinionItem opinion =
                pollModel.OpinionItems.ToList().Find(x =>
                x.OpinionName.Equals(opinionTitle)); 

            // Return null if Opinion Doesn't Exist
            if(opinion == null)
            {
                return false;
            }

            // Remove PollModel from Dataset
            getPolls.RemoveAll(x => x.PollID.Equals(pollID));

            // Remove Opinion Item from Poll Model
            pollModel.OpinionItems.ToList().Remove(opinion);

            // Update Opinion Counter
            opinion.NumCounts += 1;

            // Add Updated Opinion to PollModel
            pollModel.OpinionItems.ToList().Add(opinion);

            // Add PollModel to Dataset
            getPolls.Add(pollModel);

            // Save DataSet
            SavePollData(getPolls);

            return true;
        }

        /// <summary>
        /// Add opinion, two at a time to a poll 
        /// </summary>
        /// <param name="pollID"></param>
        /// <param name="opinionOne"></param>
        /// <param name="opinionTwo"></param>
        /// <returns></returns>
        public bool AddOpinion(int pollID, string opinionOne, string opinionTwo)
        {
            // Make sure Opinion are Not Null
            if(opinionOne == "")
            {
                return false;
            }

            // Make sure Opinion are Not Null
            if (opinionTwo == "")
            {
                return false;
            }

            // Get List of Polls from Data-Set
            List<PollModel> getPolls = GetPolls().ToList();

            // Get Poll Model based on Poll ID
            PollModel pollModel = getPolls.Find(x => x.PollID.Equals(pollID));

            //if poll does not exist, return false
            if (pollModel == null)
            {
                return false; 
            }
            //remove old poll model 
            getPolls.Remove(pollModel);

            //create new opinion items
            List<OpinionItem> getOpinions = pollModel.OpinionItems.ToList();

            //add new opinion items to list 
            getOpinions.Add(new OpinionItem(opinionOne, 0));
            getOpinions.Add(new OpinionItem(opinionTwo, 0));

            //Add new opinion items to poll item 
            pollModel.OpinionItems = getOpinions; 
 
            //Add now updated data set
            getPolls.Add(pollModel);

            // Save DataSet
            SavePollData(getPolls);

            return true;
        }

        /// <summary>
        /// Creates and Add New Poll Model to Polls Json Dataset/Database.
        /// </summary>
        /// <param name="newPoll"></param>
        /// <param name="userID"></param>
        /// <returns>PollModel</returns>
        public PollModel CreatePoll(CreatePollModel newPoll, int userID)
        {
            // Random Instance Created
            Random rnD = new Random();

            // Boolean for Duplicate Poll Existence 
            bool pollExists = PollExist(newPoll.CreateTitle);

            // True for Duplicate
            if (pollExists)
            {
                // Return Null
                return null;
            }

            // Instantiate a new Poll Model, with attributes
            var poll = new PollModel()
            {
                //user id
                UserID = userID,

                //poll id
                PollID = rnD.Next(1, 999999),

                //title of poll
                Title = newPoll.CreateTitle,

                //description of poll
                Description = newPoll.CreateDescription,

                //new list of opinion items
                OpinionItems = new List<OpinionItem>() {
                    //first opinion
                    new OpinionItem(newPoll.CreateOpinionOne, 0),
                    //second opinion
                    new OpinionItem(newPoll.CreateOpinionTwo, 0)
                }
            };

            // Get Poll Data Set
            var dataSet = GetPolls();

            // Add New Poll to Data Set
            dataSet = dataSet.Append(poll);

            // Convert List into JSON DataSet
            SavePollData(dataSet);

            // Return the Instantiated Poll Model 
            return poll;
        }
    }
}