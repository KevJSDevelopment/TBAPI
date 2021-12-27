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

        [HttpGet("/poketwitter/viewtweet/{id}")]
        public ActionResult<Tweet> GetTweetById(int id)
        {
            var tweet = _repository.GetTweetById(id);
            tweet.UserLikes = _repository.GetTweetLikes(id);
            tweet.UserRetweets = _repository.GetRetweets(id);

            return Ok(tweet);
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
        public ActionResult <UserReadDto> CreateUser(IFormFile files, IFormFile background, [FromForm] UserCreateDto userCreateDto)
        {
            var userModel = _mapper.Map<User>(userCreateDto);

            if(files != null){
                using (var target = new MemoryStream()){
                    files.CopyTo(target);
                    userModel.ImageFiles = target.ToArray();
                }
            }

            if(background != null){
                using (var target = new MemoryStream()){
                    background.CopyTo(target);
                    userModel.BackgroundImage = target.ToArray();
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

            if(tweet == null || userWhoLikedTweet == null || user == null){
                return BadRequest();
            }

            var likes = _repository.GetTweetLikes(tweet.TweetId);


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

            if(tweet == null || userWhoLikedTweet == null || user == null){
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
        public ActionResult<ICollection<Like>> GetTweetLikes(int id)
        {
            var likes = _repository.GetTweetLikes(id);

            return Ok(likes);
        }

        [HttpGet("/poketwitter/profile/tweets/{username}")]
        public ActionResult<ICollection<Tweet>> GetUserTweets(string username)
        {
            var user = _repository.GetUserByUsername(username);
            var tweets = _repository.GetUserTweets(user.UserId);

            return Ok(tweets);
        }

        [HttpGet("/poketwitter/profile/tweets+replies/{username}")]
        public ActionResult<ICollection<Tweet>> GetUserTweetsAndReplies(string username)
        {
            var user = _repository.GetUserByUsername(username);
            var tweetsAndReplies = _repository.GetUserTweetsAndReplies(user.UserId);

            return Ok(tweetsAndReplies);
        }
        [HttpGet("/poketwitter/profile/media/{username}")]
        public ActionResult<ICollection<Tweet>> GetUserMediaTweets(string username)
        {
            var user = _repository.GetUserByUsername(username);
            var mediaTweets = _repository.GetUserMediaTweets(user.UserId);

            return Ok(mediaTweets);
        }
        [HttpGet("/poketwitter/profile/likes/{username}")]
        public ActionResult<ICollection<Tweet>> GetUserLikes(string username)
        {
            var user = _repository.GetUserByUsername(username);
            var likedTweets = _repository.GetUserLikes(user.UserId);

            return Ok(likedTweets);
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
        public ActionResult<ICollection<Retweet>> GetRetweets(int id)
        {
            var retweets = _repository.GetRetweets(id);

            return Ok(retweets);
        }

        [HttpGet("/poketwitter/replies/{id}")]
        public ActionResult<ICollection<Tweet>> GetTweetReplies(int id)
        {
            var replies = _repository.GetTweetReplies(id);

            return Ok(replies);
        }

        [HttpGet("/poketwitter/wallets/{username}")]
        public ActionResult<ICollection<WalletAddress>> GetWallets(string username)
        {
            var user = _repository.GetUserByUsername(username);
            var wallets = _repository.GetWalletsByUserId(user.UserId);

            return Ok(wallets);
        }

        [HttpPost("/poketwitter/wallets")]
        public ActionResult <WalletAddress> AddWallet(WalletCreateDto walletCreateDto)
        {
            var walletAddress = _mapper.Map<WalletAddress>(walletCreateDto);

            _repository.AddWallet(walletAddress);
            _repository.SaveChanges();

            return Ok(walletAddress);
        }

        [HttpPost("/poketwitter/bookmarks")]
        public ActionResult <Bookmark> AddBookmark(BookmarkCreateDto bookmarkCreateDto)
        {
            var bookmark = _mapper.Map<Bookmark>(bookmarkCreateDto);

            _repository.AddBookmark(bookmark);
            _repository.SaveChanges();

            return Ok(bookmark);
        }

        [HttpPost("/poketwitter/messages")]
        public ActionResult <Message> AddMessage(MessageCreateDto messageCreateDto)
        {
            var message = _mapper.Map<Message>(messageCreateDto);

            _repository.AddMessage(message);
            _repository.SaveChanges();

            return Ok(message);
        }

        [HttpGet("/poketwitter/bookmarks/{username}")]
        public ActionResult<ICollection<Bookmark>> GetBookmarks(string username) {
            var user = _repository.GetUserByUsername(username);
            var bookmarkedTweets = _repository.GetBookmarks(user.UserId);
            
            return Ok(bookmarkedTweets);
        }

        [HttpGet("/poketwitter/notifications/{username}")]
        public ActionResult<ICollection<Notification>> GetNotifications(string username) {
            var user = _repository.GetUserByUsername(username);
            var notifications = _repository.GetNotifications(user.UserId);
            
            return Ok(notifications);
        }

        [HttpGet("/poketwitter/messages/{loggedInUserId}/{otherUserId}")]
        public ActionResult<ICollection<Message>> GetMessages(int loggedInUserId, int otherUserId){
            var messages = _repository.GetMessages(loggedInUserId, otherUserId);

            return Ok(messages);
        }

        [HttpPost("/poketwitter/followers")]
        public ActionResult<Follower> AddFollow(FollowerCreateDto followerCreateDto){

            var follower = _mapper.Map<Follower>(followerCreateDto);

            _repository.AddFollow(follower);
            _repository.SaveChanges();

            return Ok(follower);
        }

        [HttpDelete("/poketwitter/followers")]
        public ActionResult<Follower> DeleteFollow(FollowerCreateDto followerCreateDto){

            var follower = _mapper.Map<Follower>(followerCreateDto);

            if(follower == null)
            {
                return BadRequest();
            }

            _repository.DeleteFollow(follower);
            _repository.SaveChanges();

            return NoContent();
        }

        [HttpGet("/poketwitter/followers/{userThatFollowedId}/{userBeingFollowedId}")]
        public ActionResult<Follower> CheckFollowing(int userThatFollowedId, int userBeingFollowedId){
            var follow = _repository.CheckFollowing(userThatFollowedId, userBeingFollowedId);

            if(follow != null){
                return Ok(follow);
            }

            RequestResponse response = new RequestResponse();

            response.Status = 204;
            response.Message = "Follow not found";

            return Ok(JsonConvert.SerializeObject(response));
        }

        [HttpGet("/poketwitter/followers/{userId}")]
        public ActionResult<ICollection<Follower>> GetFollowers(int userId){
            var followers = _repository.GetFollowers(userId);

            return Ok(followers);
        }

        [HttpGet("/poketwitter/following/{userId}")]
        public ActionResult<ICollection<Follower>> GetFollowing(int userId){
            var following = _repository.GetFollowing(userId);

            return Ok(following);
        }

        
    }
}