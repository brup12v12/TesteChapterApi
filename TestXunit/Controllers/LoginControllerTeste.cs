using Charpter.WebApi.Controllers;
using Charpter.WebApi.Interfaces;
using Charpter.WebApi.Models;
using Charpter.WebApi.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace TestXunit.Controllers
{
    public class LoginControllerTeste
    {
        [Fact]
        public void LoginController_DeveRetornar_UsuarioInvalido()
        {
            //Arrange - Organização
            var fakeRepository = new Mock<IUsuarioRepository>();
            fakeRepository.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns((Usuario)null);

            LoginViewModel dadosLogin = new LoginViewModel();
            dadosLogin.Email = "email@email.com";
            dadosLogin.Senha = "12345";

            var controller = new LoginController(fakeRepository.Object);

            //Act - Execução
            var resultado = controller.Login(dadosLogin);

            //Assert - Verificação
            Assert.IsType<UnauthorizedObjectResult>(resultado);

        }

        [Fact]
        public void LoginController_DeveRetornar_Token()
        {
            //Arrange - Organização
            Usuario usuarioRetorno = new Usuario();
            usuarioRetorno.Email = "email@email.com";
            usuarioRetorno.Senha = "12345";

            var fakeRepository = new Mock<IUsuarioRepository>();
            fakeRepository.Setup(x => x.Login(It.IsAny<string>(), It.IsAny<string>())).Returns((usuarioRetorno));

            string issuerValidacao = "chapter.webapi";

            LoginViewModel dadosLogin = new LoginViewModel();
            dadosLogin.Email = "email@email.com";
            dadosLogin.Senha = "12345";

            var controller = new LoginController(fakeRepository.Object);

            //Act - Execução
            OkObjectResult resultado = (OkObjectResult)controller.Login(dadosLogin);

            string token = resultado.Value.ToString().Split(' ')[3];

            var jwtHandler = new JwtSecurityTokenHandler();
            var tokenJwt = jwtHandler.ReadJwtToken(token);

            //Assert - Verificação
            Assert.Equal(issuerValidacao, tokenJwt.Issuer);
        }


    }
}
