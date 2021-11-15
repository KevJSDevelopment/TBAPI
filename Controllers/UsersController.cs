using System.Collections.Generic;
using AutoMapper;
using TwitterBattlesAPI.Data;
using TwitterBattlesAPI.Dtos;
using TwitterBattlesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;
using Microsoft.AspNetCore.Http;
using System.IO;
using Newtonsoft.Json;
using TwitterBattlesAPI.HelperClasses;

namespace TwitterBattlesAPI.Controllers
{
    [Route("poketwitter")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUserRepo _repository;
        private readonly IMapper _mapper;

        public UsersController(IUserRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        //GET poketwitter
        [HttpGet]
        public ActionResult<ICollection<UserCreateDto>> GetAllUsers()
        {
            var userItems = _repository.GetAllUsers();

            return Ok(_mapper.Map<ICollection<UserCreateDto>>(userItems));
        }

        //GET poketwitter/{Username}
        [HttpGet("{Username}", Name="GetUserByUsername")]

        public ActionResult<UserCreateDto> GetUserByUsername(string username)
        {
            var userItem = _repository.GetUserByUsername(username);

            if(userItem != null){
                return Ok(_mapper.Map<UserCreateDto>(userItem));
            }

            RequestResponse response = new RequestResponse();
            response.Status = 400;
            response.Message = "Username is incorrect";

            return BadRequest(error: JsonConvert.SerializeObject(response));
        }

        //GET poketwitter/tweet/{id}
        [HttpGet("/poketwitter/tweets/{id}", Name="GetUserById")]
        public ActionResult<UserCreateDto> GetUserById(int id)
        {
            var userItem = _repository.GetUserById(id);

            if(userItem != null){
                return Ok(_mapper.Map<UserCreateDto>(userItem));
            }

            RequestResponse response = new RequestResponse();
            response.Status = 400;
            response.Message = "Id is incorrect";

            return BadRequest(error: JsonConvert.SerializeObject(response));
        }

        [HttpPost("/login")]
        public ActionResult<User> Login(UserLoginDto userLoginDto)
        {
            var user = _repository.GetUserByUsername(userLoginDto.Username);
            if(user != null && user.Password == userLoginDto.Password){
                return Ok(_mapper.Map<UserReadDto>(user));
            }

            RequestResponse response = new RequestResponse();
            response.Status = 400;
            response.Message = "Username or password is incorrect";

            return BadRequest(error: JsonConvert.SerializeObject(response));
        }
        
        [HttpGet("/{username}")]
        public ActionResult<ICollection<Tweet>> GetTweets(string username)
        {
            var user = _repository.GetUserByUsername(username);

            var tweets = _repository.GetTweets(user.UserId);

            return Ok(tweets);
        }
        [HttpGet("/home/{username}")]
        public ActionResult<ICollection<Tweet>> GetTweetFeed(string username)
        {
            var user = _repository.GetUserByUsername(username);

            var tweets = _repository.GetTweetFeed(user.UserId);

            return Ok(tweets);
        }

        //POST poketwitter/{username} --add tweet
        [HttpPost("{Username}")]
        public ActionResult<Tweet> AddTweet([FromRoute] string username, IFormFile files, [FromForm] TweetCreateDto tweetCreateDto)
        {
            var userItem = _repository.GetUserByUsername(username);

            if(userItem != null && tweetCreateDto != null){

                var newTweet = _mapper.Map<Tweet>(tweetCreateDto);
                newTweet.UserId = userItem.UserId;
                newTweet.CreatedDate = DateTime.Now;
                if(files != null){
                    using (var target = new MemoryStream()){
                        files.CopyTo(target);
                        newTweet.Media = target.ToArray();
                    }
                }
                _repository.AddTweet(newTweet);
                _repository.SaveChanges();

                return Ok(newTweet);
            }
            
            return BadRequest();
        }
        //POST poketwitter
        [HttpPost]
        public ActionResult <UserReadDto> CreateUser(IFormFile files, [FromForm] UserCreateDto userCreateDto)
        {
            var userModel = _mapper.Map<User>(userCreateDto);

            if(files != null){
                using (var target = new MemoryStream()){
                    files.CopyTo(target);
                    userModel.ImageFiles = target.ToArray();
                }
            }
            _repository.CreateUser(userModel);
            _repository.SaveChanges();

            var userReadDto = _mapper.Map<UserReadDto>(userModel);

            return CreatedAtRoute(nameof(GetUserByUsername), new {Username = userReadDto.Username}, userReadDto);
        }

        //PUT poketwitter/{username}
        [HttpPut("{username}")]
        public ActionResult UpdateUser(string username, UserUpdateDto userUpdateDto, IFormFile files)
        {
            var userModelFromRepo = _repository.GetUserByUsername(username);

            if(userModelFromRepo == null)
            {
                return BadRequest();
            }

            if(files != null){
                using (var target = new MemoryStream()){
                    files.CopyTo(target);
                    userModelFromRepo.ImageFiles = target.ToArray();
                }
            }

            _mapper.Map(userUpdateDto, userModelFromRepo);

            _repository.UpdateUser(userModelFromRepo);

            _repository.SaveChanges();

            return NoContent();
        }

        //PATCH poketwitter/{username}
        [HttpPatch("{username}")]
        public ActionResult PartialUserUpdate(string username, JsonPatchDocument<UserUpdateDto> patchDoc, IFormFile files)
        {

            var userModelFromRepo = _repository.GetUserByUsername(username);

            if(userModelFromRepo == null)
            {
                return BadRequest();
            }

            if(files != null){
                using (var target = new MemoryStream()){
                    files.CopyTo(target);
                    userModelFromRepo.ImageFiles = target.ToArray();
                }
            }

            var userToPatch = _mapper.Map<UserUpdateDto>(userModelFromRepo);
            patchDoc.ApplyTo(userToPatch, ModelState);
            
            if(!TryValidateModel(userToPatch))
            {
                return ValidationProblem(ModelState);
            }

            _mapper.Map(userToPatch, userModelFromRepo);

            _repository.UpdateUser(userModelFromRepo);

            _repository.SaveChanges();

            return NoContent();

        }

        [HttpDelete("{username}")]
        public ActionResult DeleteUser(string username)
        {
            var userModelFromRepo = _repository.GetUserByUsername(username);
            
            if(userModelFromRepo == null)
            {
                return BadRequest();
            }

            _repository.DeleteUser(userModelFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }

        [HttpDelete("/poketwitter/tweets/{id}")]
        public ActionResult DeleteTweet(int id)
        {
            var tweetModel = _repository.GetTweetById(id);
            
            if(tweetModel == null)
            {
                return BadRequest();
            }

            _repository.DeleteTweet(tweetModel);
            _repository.SaveChanges();

            return NoContent();
        }

        [HttpPost("/poketwitter/liketweet/{id}")]
        public ActionResult<Like> LikeTweet(int id, TweetReadDto tweetReadDto)
        {
            var tweet = _repository.GetTweetById(tweetReadDto.TweetId);
            var user = _repository.GetUserById(tweet.UserId);
            var userWhoLikedTweet = _repository.GetUserById(id);

            if(userWhoLikedTweet.Username == user.Username || tweet == null || userWhoLikedTweet == null || user == null){
                return BadRequest();
            }

            var likes = _repository.GetLikes(tweet.TweetId);


            foreach (var likeObj in likes)
            {
                if(likeObj.TweetId == tweet.TweetId && likeObj.UserId == userWhoLikedTweet.UserId){
                    _repository.UnlikeTweet(likeObj);
                    _repository.SaveChanges();


                    RequestResponse requestResponse = new RequestResponse();
                    requestResponse.Status = 204;
                    requestResponse.Message = "unliked tweet";

                    return Ok(JsonConvert.SerializeObject(requestResponse));
                }
            }

            
            var like = new Like();
            
            like.User = userWhoLikedTweet;
            like.Tweet = tweet;

            _repository.LikeTweet(like);
            _repository.SaveChanges();

            return Ok(like);
        }

        [HttpPost("/poketwitter/retweets/{id}")]
        public ActionResult<Retweet> Retweet(int id, TweetReadDto tweetReadDto)
        {
            var tweet = _repository.GetTweetById(tweetReadDto.TweetId);
            var user = _repository.GetUserById(tweet.UserId);
            var userWhoLikedTweet = _repository.GetUserById(id);

            if(userWhoLikedTweet.Username == user.Username || tweet == null || userWhoLikedTweet == null || user == null){
                return BadRequest();
            }

            var retweets = _repository.GetRetweets(tweet.TweetId);


            foreach (var retweetObject in retweets)
            {
                if(retweetObject.TweetId == tweet.TweetId && retweetObject.UserId == userWhoLikedTweet.UserId){
                    _repository.Unretweet(retweetObject);
                    _repository.SaveChanges();

                    RequestResponse requestResponse = new RequestResponse();
                    requestResponse.Status = 204;
                    requestResponse.Message = "unretweeted";

                    return Ok(JsonConvert.SerializeObject(requestResponse));
                }
            }

            var retweet = new Retweet();

            retweet.UserId = userWhoLikedTweet.UserId;
            retweet.User = userWhoLikedTweet;
            retweet.TweetId = tweet.TweetId;
            retweet.Tweet = tweet;
            
            _repository.Retweet(retweet);
            _repository.SaveChanges();

            return Ok(retweet);
        }

        [HttpPost("/poketwitter/quotetweet/{id}")]
        public ActionResult<QuoteTweet> QuoteTweet(int id, TweetReadDto tweetReadDto)
        {
            var tweet = _repository.GetTweetById(tweetReadDto.TweetId);
            var user = _repository.GetUserById(tweet.UserId);
            var userWhoLikedTweet = _repository.GetUserById(id);

            if(userWhoLikedTweet.Username == user.Username || tweet == null || userWhoLikedTweet == null || user == null){
                return BadRequest();
            }

            var quoteTweet = new QuoteTweet();

            quoteTweet.UserId = userWhoLikedTweet.UserId;
            quoteTweet.User = userWhoLikedTweet;
            quoteTweet.TweetId = tweet.TweetId;
            quoteTweet.Tweet = tweet;
            quoteTweet.Message = tweet.Message;
            
            _repository.QuoteTweet(quoteTweet);
            _repository.SaveChanges();

            return Ok();
        }

        [HttpPost("/poketwitter/checklike/{id}")]
        public ActionResult<Like> CheckLike(int id, TweetReadDto tweetReadDto)
        {
            Like like = _repository.CheckLike(id, tweetReadDto.TweetId);

            if(like != null){
                return Ok(like);
            }

            RequestResponse response = new RequestResponse();

            response.Status = 204;
            response.Message = "Like not found";

            return Ok(JsonConvert.SerializeObject(response));
        }


        [HttpGet("/poketwitter/likes/{id}")]
        public ActionResult<ICollection<UserReadDto>> GetLikes(int id)
        {
            var userItems = _repository.GetLikes(id);

            return Ok(userItems);
        }

        [HttpPost("/poketwitter/checkretweet/{id}")]
        public ActionResult<Retweet> CheckRetweet(int id, TweetReadDto tweetReadDto)
        {
            Retweet retweet = _repository.CheckRetweet(id, tweetReadDto.TweetId);

            if(retweet != null){
                return Ok(retweet);
            }

            RequestResponse response = new RequestResponse();

            response.Status = 204;
            response.Message = "Retweet not found";

            return Ok(JsonConvert.SerializeObject(response));
        }


        [HttpGet("/poketwitter/retweets/{id}")]
        public ActionResult<ICollection<UserReadDto>> GetRetweets(int id)
        {
            var userItems = _repository.GetRetweets(id);

            return Ok(userItems);
        }
    }
}