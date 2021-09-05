using System.Collections.Generic;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using TwitterBattlesAPI.Data;
using TwitterBattlesAPI.Dtos;
using TwitterBattlesAPI.Models;

namespace TwitterBattlesAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TweetsController : ControllerBase
    {
        private readonly ITweetRepo _repository;
        private readonly IMapper _mapper;

        public TweetsController(ITweetRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //GET api/tweets
        [HttpGet]
        public ActionResult<IEnumerable<TweetCreateDto>> GetAllTweets()
        {
            var tweetItems = _repository.GetAllTweets();

            return Ok(_mapper.Map<IEnumerable<TweetCreateDto>>(tweetItems));
        }

        //GET api/tweets/{id}
        [HttpGet("{id}", Name="GetTweetById")]
        public ActionResult<TweetCreateDto> GetTweet(int id)
        {
            var tweetItem = _repository.GetTweetById(id);

            if(tweetItem != null){
                return Ok(_mapper.Map<TweetCreateDto>(tweetItem));
            }

            return NotFound();
        }

        //POST api/tweets
        [HttpPost]
        public ActionResult <TweetReadDto> CreateTweet(TweetCreateDto tweetCreateDto)
        {
            var tweetModel = _mapper.Map<Tweet>(tweetCreateDto);
            _repository.CreateTweet(tweetModel);
            _repository.SaveChanges();

            // var tweetReadDto = _mapper.Map<TweetReadDto>(tweetModel);

            return CreatedAtRoute(nameof(GetTweet), new {Id = tweetModel.Id}, tweetModel);
        }

        [HttpDelete("{id}")]
        public ActionResult DeleteTweet(int id)
        {
            var tweetModelFromRepo = _repository.GetTweetById(id);
            
            if(tweetModelFromRepo == null)
            {
                return NotFound();
            }

            _repository.DeleteTweet(tweetModelFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }
    }
}