using System.Collections.Generic;
using AutoMapper;
using TwitterBattlesAPI.Data;
using TwitterBattlesAPI.Dtos;
using TwitterBattlesAPI.Models;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using System;

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

        //GET api/Users
        [HttpGet]
        public ActionResult<ICollection<UserCreateDto>> GetAllUsers()
        {
            var userItems = _repository.GetAllUsers();

            return Ok(_mapper.Map<ICollection<UserCreateDto>>(userItems));
        }

        //GET api/Users/{Username}
        [HttpGet("{Username}", Name="GetUserByUsername")]
        public ActionResult<UserCreateDto> GetUserByUsername(string username)
        {
            var userItem = _repository.GetUserByUsername(username);

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

        [HttpPost("{Username}")]
        public ActionResult<Tweet> AddTweet([FromRoute] string username, [FromBody] TweetCreateDto tweetCreateDto)
        {
            var userItem = _repository.GetUserByUsername(username);

            if(userItem != null && tweetCreateDto != null){
                var newTweet = _mapper.Map<Tweet>(tweetCreateDto);
                newTweet.UserId = userItem.Id;
                newTweet.CreatedDate = DateTime.Now;
                _repository.AddTweet(newTweet);
                _repository.SaveChanges();

                return Ok(newTweet);
            }
            
            return NotFound();

        }
        //POST api/Users
        [HttpPost]
        public ActionResult <UserReadDto> CreateUser(UserCreateDto userCreateDto)
        {
            var userModel = _mapper.Map<User>(userCreateDto);
            _repository.CreateUser(userModel);
            _repository.SaveChanges();

            var userReadDto = _mapper.Map<UserReadDto>(userModel);

            return CreatedAtRoute(nameof(GetUserByUsername), new {Username = userReadDto.Username}, userReadDto);
        }

        //PUT api/Users/{username}
        [HttpPut("{username}")]
        public ActionResult UpdateUser(string username, UserUpdateDto userUpdateDto)
        {
            var userModelFromRepo = _repository.GetUserByUsername(username);

            if(userModelFromRepo == null)
            {
                return NotFound();
            }

            _mapper.Map(userUpdateDto, userModelFromRepo);

            _repository.UpdateUser(userModelFromRepo);

            _repository.SaveChanges();

            return NoContent();
        }

        //PATCH api/users/{username}
        [HttpPatch("{username}")]
        public ActionResult PartialUserUpdate(string username, JsonPatchDocument<UserUpdateDto> patchDoc)
        {

            var userModelFromRepo = _repository.GetUserByUsername(username);

            if(userModelFromRepo == null)
            {
                return NotFound();
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
    }
}