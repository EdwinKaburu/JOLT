﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using System.IO;
using System.Text.Json;

using ContosoCrafts.WebSite.Models;
using Microsoft.AspNetCore.Hosting;

namespace ContosoCrafts.WebSite.Services
{
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
            get
            {
                return Path.Combine(WebHostEnvironment.WebRootPath,
              "data", "polls.json");
            }
        }

        /// <summary>
        /// Deserialize a Json of Polls to List
        /// </summary>
        /// <returns></returns>
        public IEnumerable<PollModel> GetPolls()
        {
            //create file reader for Json file 
            using (var jsonFileReader = File.OpenText(JsonFileName))
            {
                // Deserialize Json to List
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
                // Serialize Collection to Json
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
        
        public PollModel GetPoll(int pollID)
        {
            List<PollModel> polls = GetPolls().ToList();

            return polls.Find(x => x.PollID == pollID);
        }

        public PollModel GetPoll(string inputTitle)
        {
            List<PollModel> polls = GetPolls().ToList();

            return polls.Find(x => x.Title == inputTitle);
        }

        public bool PollExist(string inputTitle)
        {
            if(GetPoll(inputTitle) == null)
            {
                return false;
            }

            return true;
        }

        public bool PollExist(int inputID)
        {
            if (GetPoll(inputID) == null)
            {
                return false;
            }
            return true;
        }



        public PollModel CreatePoll(CreatePollModel newPoll, int userID)
        {
            // Make Sure Poll is Unique
            bool pollExists = PollExist(newPoll.CreateTitle);
            
            if(pollExists)
            {
                return null;
            }
           
            var poll = new PollModel()
            {
                UserID = userID,

                PollID = GetPolls().Count(),

                Title = newPoll.CreateTitle,

                Description = newPoll.CreateDescription,

                OpinionItems = new List<OpinionItem> () {
                    new OpinionItem(newPoll.CreateOpinionOne, 0), new OpinionItem(newPoll.CreateOpinionTwo, 0)
                }
            };

            // Get Poll Data Set
            var dataSet = GetPolls();

            // Add New Poll to Data Set
            dataSet = dataSet.Append(poll);

            // Convert List into Json DataSet
            SavePollData(dataSet);

            return poll;
        }
    }
}
