﻿using api.Dtos;
using api.Errors;
using api.Extensions;
using AutoMapper;
using Core.Entities.Identity;
using Core.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace api.Controllers
{
    public class AccountController : BaseApiController
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;
        private readonly IMapper _mapper;

        public AccountController(UserManager<AppUser> userManager,SignInManager<AppUser> signInManager,ITokenService tokenService,IMapper mapper)
        {

            _userManager = userManager;
            _signInManager = signInManager;
            _tokenService = tokenService;
            _mapper = mapper;
        }
        [HttpGet]
        [Authorize]
        public async Task<ActionResult<UserDto>> GetCurrentUser()
        {
            //var email = User.FindFirstValue(ClaimTypes.Email);
            var user=await _userManager.FindByEmailFromClaimPrinciples(User);
            return new UserDto() { 
                Email = user.Email,
                Token=_tokenService.CreateToken(user),
                Displayname = user.UserName,
            };
        }        


        [HttpPost("login")]
        public async Task<ActionResult<UserDto>> Login(LoginDto loginDto)
        {
            var user=await _userManager.FindByEmailAsync(loginDto.Email);
            if(user == null)
            {
                return Unauthorized(new ApiResponse(401));
            }

            var result=await _signInManager.CheckPasswordSignInAsync(user, loginDto.Password,false);

            if (!result.Succeeded)
            {
                return Unauthorized(new ApiResponse(401));
            }
            return new UserDto() {
                Email=user.Email,
                Displayname=user.UserName,
                Token=_tokenService.CreateToken(user)
            };
        }

        [HttpPost("register")]
        public async Task<ActionResult<UserDto>> Register(RegisterDto registerDto)
        {
            if (CheckEmailExistsAsync(registerDto.Email).Result.Value)
            {
                return new BadRequestObjectResult(new ApiValidationErrorResponse() {
                    Errors = new[] {"Email address is already in use"}
                });
            }

            var user = new AppUser()
            {
                DisplayName = registerDto.Displayname,
                Email = registerDto.Email,
                UserName = registerDto.Email
            };
            var result = await _userManager.CreateAsync(user, registerDto.Password);
            if (!result.Succeeded)
            {
                return BadRequest(new ApiResponse(400));
            }

            return new UserDto()
            {
                Email=user.Email,
                Displayname=user.UserName,
                Token=_tokenService.CreateToken(user)
            };
        }

        [HttpGet("emailexists")]
        public async Task<ActionResult<bool>> CheckEmailExistsAsync([FromQuery] string email)
        {
            return await _userManager.FindByEmailAsync(email) != null;
        }
        [HttpGet("address")]
        public async Task<ActionResult<AddressDto>> GetUserAddress()
        {
            //var email = User.FindFirstValue(ClaimTypes.Email);

            var user = await _userManager.FindUserByClaimPrincipleWithAddress(User);
            return _mapper.Map<Address,AddressDto>(user.Address);
        }
        [Authorize]
        [HttpPut("address")]
        public async Task<ActionResult<AddressDto>> UpdateAddress(AddressDto addressDto)
        {
            var user = await _userManager.FindUserByClaimPrincipleWithAddress(HttpContext.User);
            user.Address = _mapper.Map<AddressDto, Address>(addressDto);

            var result=await _userManager.UpdateAsync(user);
            if(result.Succeeded) {
                return Ok(_mapper.Map<Address, AddressDto>(user.Address));
            }
            return BadRequest("Problem while updating user");

        }

    }
}
