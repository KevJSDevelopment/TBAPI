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

            return NotFound();
        }

        //GET poketwitter/tweet/{id}
        [HttpGet("/poketwitter/tweets/{id}", Name="GetUserById")]
        public ActionResult<UserCreateDto> GetUserById(int id)
        {
            var userItem = _repository.GetUserById(id);

            if(userItem != null){
                return Ok(_mapper.Map<UserCreateDto>(userItem));
            }

            return NotFound();
        }
        
        [HttpGet("/tweets")]
        public ActionResult<ICollection<Tweet>> GetTweets()
        {
            var tweets = _repository.GetTweets();

            return Ok(tweets);
        }

        //POST poketwitter/{username} --add tweet
        [HttpPost("{Username}")]
        public ActionResult<Tweet> AddTweet([FromRoute] string username, IFormFile files, [FromForm] TweetCreateDto tweetCreateDto)
        {
            var userItem = _repository.GetUserByUsername(username);

            if(userItem != null && tweetCreateDto != null){

                var newTweet = _mapper.Map<Tweet>(tweetCreateDto);
                newTweet.UserId = userItem.Id;
                newTweet.CreatedDate = DateTime.Now;
                if(files != null){
                    using (var target = new MemoryStream()){
                        files.CopyTo(target);
                        newTweet.media = target.ToArray();
                    }
                }
                _repository.AddTweet(newTweet);
                _repository.SaveChanges();

                return Ok(newTweet);
            }
            
            return NotFound();

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
                return NotFound();
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
                return NotFound();
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
                return NotFound();
            }

            _repository.DeleteUser(userModelFromRepo);
            _repository.SaveChanges();

            return NoContent();
        }

        [HttpDelete("/poketwitter/tweets/{id}")]
        public ActionResult DeleteUser(int id)
        {
            var tweetModel = _repository.GetTweetById(id);
            
            if(tweetModel == null)
            {
                return NotFound();
            }

            _repository.DeleteTweet(tweetModel);
            _repository.SaveChanges();

            return NoContent();
        }
    }
}