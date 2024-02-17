using Hirportal.Controllers;
using Hirportal.Models;
using HirportalData;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Xunit;

namespace HirportalTest
{
    public class UnitTest1 : IDisposable
    {
        private readonly HirportalContext context;
        private readonly List<UserDTO> userDTOs;
        private readonly List<ArticlesDTO> articleDTOs;
        private readonly List<ImageDTO> imagesDTOs;
        public UnitTest1()
        {
            var serviceProvider = new ServiceCollection()
                    .AddEntityFrameworkInMemoryDatabase()
                    .BuildServiceProvider();

            var options = new DbContextOptionsBuilder<HirportalContext>()
                .UseInMemoryDatabase("HirportalTest")
                .UseInternalServiceProvider(serviceProvider)
                .Options;

            context = new HirportalContext(options);
            context.Database.EnsureCreated();

            // adatok inicializációja
            var testuser = new List<User>
            {
                new User
                {
                    UserName = "test",
                    Name = "test"
                    
                }

            };
            context.Users.AddRange(testuser);
            var articles = new List<Article>
            {
                new Article
                {
                    UserId=1,
                    Summary="TEstSum1",
                    Title="TestTitle1",
                    Content="TestContent1",
                    IsMainArticle=true,
                    Date=new DateTime(2019,5,2)

                },
                new Article
                {
                    UserId=1,
                    Summary="TEstSum2",
                    Title="TestTitle2",
                    Content="TestContent2",
                    IsMainArticle=false,
                    Date=new DateTime(2019,2,2)

                }
            };
           
            context.Articles.AddRange(articles);

            var images = new List<Picture>
            {
                new Picture
                {
                    ArticleId=1,
                    Image=new Byte[]{1,1,4,7,8,0,1}
                }
            };
            context.Pictures.AddRange(images);
            context.SaveChanges();

            userDTOs = testuser.Select(user => new UserDTO
            {
                Id = user.Id,
                Name = user.Name,
                Password="aaa"
            }).ToList();
            articleDTOs = articles.Select(article=> new ArticlesDTO
            {
                Id = article.Id,
                Title = article.Title,
                Content = article.Content,
                Summary = article.Summary,
                IsMainArticle = article.IsMainArticle,
                User = new UserDTO {Id=1, Name="Test Elek"}

            }).ToList();

            imagesDTOs = images.Select(image => new ImageDTO
            {
                Id=image.Id,
                Image=image.Image,
                ArticleId=image.ArticleId
            }).ToList();
          
    }
        public void Dispose()
        {
            context.Database.EnsureDeleted();
            context.Dispose();
        }

        [Fact]
        public void GetArticles()
        {
            var controller = new ArticlesController(context);
            var result = controller.GetArticles(1);

            var objectResult = Assert.IsType<OkObjectResult>(result);

            result = controller.GetArticles(2);
            objectResult = Assert.IsType<OkObjectResult>(result);
        }
        [Fact]
        public void PostImage()
        {
            var controller = new ArticleImagesController(context);
            var image = new ImageDTO
            {
                ArticleId = 1,
                Image = new Byte[] { 0, 1, 4 }
            };

            var result = controller.PostImage(image);

            var objectResult = Assert.IsType<StatusCodeResult>(result);
            //Assert.Equal(imagesDTOs.Count, context.Pictures.Count());
        }
        [Fact]
        public void DeleteImage()
        {
            var controller = new ArticleImagesController(context);
            

            var result = controller.DeleteImage(1);

            var objectResult = Assert.IsType<OkResult>(result);
            //Assert.Equal(imagesDTOs.Count, context.Pictures.Count());
        }
        [Fact]
        public void GetMoreArticles()
        {
            //sikerult megoldani
            var userStoreMock = new Mock<IUserStore<User>>();
            var test = new Mock<UserManager<User>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
            test.Setup(s => s.FindByNameAsync("test")).ReturnsAsync(new User { Id = 1, Name = "test" });
            var controller = new ArticlesController(context);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1")
            }));
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            var result = controller.GetArticles();

            var objectResult = Assert.IsType<OkObjectResult>(result);//megyen
            var model = Assert.IsAssignableFrom<IEnumerable<ArticlesDTO>>(objectResult.Value);
            Assert.Equal(articleDTOs, model);
        }
        
        [Fact]
        public void PostArticles()
        {

            //sikerult megoldani
            var userStoreMock = new Mock<IUserStore<User>>();
            var test = new Mock<UserManager<User>>(
                userStoreMock.Object, null, null, null, null, null, null, null, null);
            test.Setup(s => s.FindByNameAsync("test")).ReturnsAsync(new User { Id = 1, Name = "test" });
            var controller = new ArticlesController(context);
            var user = new ClaimsPrincipal(new ClaimsIdentity(new Claim[]
            {
                new Claim(ClaimTypes.NameIdentifier, "1")
            }));
            controller.ControllerContext = new ControllerContext()
            {
                HttpContext = new DefaultHttpContext() { User = user }
            };
            var newarticle = new ArticlesDTO
            {
                Title = "Ujtest",
                Content = "UJJJJJ",
                Date = new DateTime(2019, 02, 8, 12, 00, 00),
                Summary = "UJJJJJJ",
                IsMainArticle = false

            };
            var result = controller.PostArticle(newarticle);
            var objres= Assert.IsType<CreatedAtRouteResult>(result);
            Assert.Equal(articleDTOs.Count+1, context.Articles.Count());
            //megyen kösz
        }

        [Fact]
        public void PutArticles()
        {
            var controller = new ArticlesController(context);
            var newarticle = new ArticlesDTO
            {
                Id=1,
                Title = "Ujupdate",
                Content = "UJJJJJ",
                Summary = "UJJJJJJ",
                IsMainArticle = false,
                User = new UserDTO { Id = 1, Name = "Test Elek" }
            };


            var result = controller.PutArticle(newarticle);

            // Assert
            var objectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(articleDTOs.Count, context.Articles.Count());
            //Assert.Equal(newarticle, model);

        }
        [Fact]
        public void DeleteArticle()
        {
            var controller = new ArticlesController(context);
            var result = controller.DeleteArticle(1);

            // Assert
            var objectResult = Assert.IsType<OkResult>(result);
            Assert.Equal(articleDTOs.Count-1, context.Articles.Count());
            //Assert.Equal(newarticle, model);

        }
    }
}
