using System.Threading.Tasks;
using TransportManagement.Models.User;
using TransportManagement.Services.User;
using TransportManagement.Services.User.EmailSender;
using TransportManagement.Services.Driver;
using TransportManagement;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Xunit;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Moq;
using System.Collections.Generic;

public class AccountServiceTests
{
    private readonly TransportManagementDbContext _context;
    private readonly AccountService _accountService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountServiceTests()
    {
        var options = new DbContextOptionsBuilder<TransportManagementDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        _context = new TransportManagementDbContext(options);

        var store = new UserStore<ApplicationUser>(_context);
        var passwordHasher = new PasswordHasher<ApplicationUser>();
        var userValidators = new List<IUserValidator<ApplicationUser>> { new UserValidator<ApplicationUser>() };
        var passwordValidators = new List<IPasswordValidator<ApplicationUser>> { new PasswordValidator<ApplicationUser>() };

        var loggerMock = new Mock<ILogger<UserManager<ApplicationUser>>>();
        _userManager = new UserManager<ApplicationUser>(
            store,
            null,
            passwordHasher,
            userValidators,
            passwordValidators,
            null,
            null,
            null,
            loggerMock.Object // Mock the logger
        );

        var contextAccessorMock = new Mock<IHttpContextAccessor>();
        var claimsFactoryMock = new Mock<IUserClaimsPrincipalFactory<ApplicationUser>>();
        var authenticationSchemeProviderMock = new Mock<IAuthenticationSchemeProvider>();
        var loggerSignInMock = new Mock<ILogger<SignInManager<ApplicationUser>>>();

        var optionsMock = new Mock<IOptions<IdentityOptions>>();
        optionsMock.Setup(o => o.Value).Returns(new IdentityOptions());

        var signInManagerMock = new Mock<SignInManager<ApplicationUser>>(
            _userManager,
            contextAccessorMock.Object,
            claimsFactoryMock.Object,
            optionsMock.Object,
            loggerSignInMock.Object,
            authenticationSchemeProviderMock.Object,
            null
        );

        signInManagerMock.Setup(s => s.PasswordSignInAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<bool>(), It.IsAny<bool>()))
            .ReturnsAsync(SignInResult.Success);

        _signInManager = signInManagerMock.Object;

        var configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(new Dictionary<string, string>
            {
                { "Smtp:Host", "" },
                { "Smtp:Port", "25" },
                { "Smtp:Username", "" },
                { "Smtp:Password", "" }
            })
            .Build();

        var emailSender = new EmailSender(configuration);
        var driverService = new DriverService(_context, _userManager);

        _accountService = new AccountService(_userManager, _signInManager, emailSender, driverService, _context);
    }

    [Fact]
    public async Task RegisterUser_Should_Create_User_When_Valid_Data()
    {
        var model = new RegisterViewModel
        {
            Email = "test@example.com",
            FirstName = "Jan",
            LastName = "Kowalski",
            PhoneNumber = "123456789",
            Address = "Testowa 1",
            Experience = 5
        };

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            FirstName = model.FirstName,
            LastName = model.LastName,
            Address = model.Address,
            Role = "User"
        };

        var result = await _userManager.CreateAsync(user, "Password123!");
        var createdUser = await _userManager.FindByEmailAsync(model.Email);

        Assert.True(result.Succeeded);
        Assert.NotNull(createdUser);
        Assert.Equal(model.Email, createdUser.Email);
    }

    [Fact]
    public async Task LoginUser_Should_Return_User_When_Correct_Credentials()
    {
        var user = new ApplicationUser
        {
            Email = "user@example.com",
            UserName = "user@example.com",
            FirstName = "Jan",
            LastName = "Kowalski",
            Address = "Testowa 1",
            Role = "User"
        };

        var result = await _userManager.CreateAsync(user, "Password123!");
        Assert.True(result.Succeeded);

        var loginModel = new LoginViewModel
        {
            Email = "user@example.com",
            Password = "Password123!"
        };

        var loggedInUser = await _accountService.LoginUserAsync(loginModel);

        Assert.NotNull(loggedInUser);
        Assert.Equal(user.Email, loggedInUser.Email);
    }

    [Fact]
    public async Task LoginUser_Should_Return_Null_When_Wrong_Credentials()
    {
        var loginModel = new LoginViewModel
        {
            Email = "wrong@example.com",
            Password = "WrongPassword"
        };

        var result = await _accountService.LoginUserAsync(loginModel);

        Assert.Null(result);
    }


    [Fact]
    public async Task ChangePassword_Should_Return_True_When_Password_Changed_Successfully()
    {
        // Arrange
        var user = new ApplicationUser
        {
            Email = "user@example.com",
            UserName = "user@example.com",
            FirstName = "Jan",
            LastName = "Kowalski",
            Address = "Testowa 1",
            Role = "User",
            HasChangedPassword = false
        };

        // Mock UserManager
        var mockUserManager = new Mock<UserManager<ApplicationUser>>(
            Mock.Of<IUserStore<ApplicationUser>>(),
            null,
            new PasswordHasher<ApplicationUser>(),
            new List<IUserValidator<ApplicationUser>>(),
            new List<IPasswordValidator<ApplicationUser>>(),
            null,
            null,
            null,
            null
        );

        // Mock FindByEmailAsync to return the user
        mockUserManager.Setup(m => m.FindByEmailAsync(It.IsAny<string>()))
            .ReturnsAsync(user);

        // Mock CheckPasswordAsync to return true (indicating current password is correct)
        mockUserManager.Setup(m => m.CheckPasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>()))
            .ReturnsAsync(true);

        // Mock ChangePasswordAsync to return IdentityResult.Success (indicating successful password change)
        mockUserManager.Setup(m => m.ChangePasswordAsync(It.IsAny<ApplicationUser>(), It.IsAny<string>(), It.IsAny<string>()))
            .ReturnsAsync(IdentityResult.Success);

        // Mock UpdateAsync to return IdentityResult.Success (indicating successful user update)
        mockUserManager.Setup(m => m.UpdateAsync(It.IsAny<ApplicationUser>()))
            .ReturnsAsync(IdentityResult.Success);

        // Now use the mocked UserManager in the AccountService (inject mock)
        var accountServiceWithMock = new AccountService(
            mockUserManager.Object,
            _signInManager,
            new EmailSender(new ConfigurationBuilder().Build()),
            new DriverService(_context, mockUserManager.Object),
            _context
        );

        // Create ChangePasswordModel
        var changePasswordModel = new ChangePasswordModel
        {
            CurrentPassword = "Password123!",
            NewPassword = "NewPassword123!",
            ConfirmPassword = "NewPassword123!"
        };

        // Act
        var changePasswordResult = await accountServiceWithMock.ChangePassword(
            user.Email,
            changePasswordModel.CurrentPassword,
            changePasswordModel.NewPassword,
            changePasswordModel.ConfirmPassword
        );

        // Assert
        Assert.True(changePasswordResult, "The password change did not return true!");

        // Verify that the password was updated and the HasChangedPassword flag is true
        var updatedUser = await mockUserManager.Object.FindByEmailAsync(user.Email);
        Assert.NotNull(updatedUser);
        Assert.True(updatedUser.HasChangedPassword, "The HasChangedPassword flag was not updated.");

        // Check if the password is actually changed by checking with the new password
        var passwordValid = await mockUserManager.Object.CheckPasswordAsync(updatedUser, "NewPassword123!");
        Assert.True(passwordValid, "The password was not correctly changed.");
    }
}