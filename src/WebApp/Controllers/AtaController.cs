﻿using Microsoft.AspNetCore.Mvc;

namespace WebApp.Controllers
{
    public class AtaController : Controller
    {
        public IActionResult Cadastrar()
        {
            return View();
        }

        public IActionResult IncluirDetentora() => View();
        public IActionResult IncluirParticipante() => View();
    }
}
