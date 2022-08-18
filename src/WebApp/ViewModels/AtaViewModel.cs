﻿using Domain.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace WebApp.ViewModels
{
    public class AtaViewModel
    {
        public AtaViewModel()
        {
            DataCadastro = DateTime.Now;
            LimiteAdesao = 100;
        }

        [Display(Name = "Código da Ata")]
        [Required(ErrorMessage = "Campo Obrigatório!")]
        public int CodigoAta { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório!")]
        [Display(Name = "Ano da Ata")]
        public int AnoAta { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório!")]
        [Display(Name = "Número do Processo")]
        [MinLength(18, ErrorMessage = "Número de {0} inválido!")]
        public string NumeroProcesso { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório!")]
        [Display(Name = "Tipo de Pregão")]
        public EPregao TipoPregao { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório!")]
        [Display(Name = "Número do Pregão")]
        public string NumeroPregao { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório!")]
        [Display(Name = "Ano do Pregão")]
        public int AnoPregao { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório!")]
        [Display(Name = "Data de Homologação")]
        public DateTime DataHomologacao { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório!")]
        [Display(Name = "Data da Publicação no DOE")]
        public DateTime DataPublicacaoDOE { get; set; }

        [Required(ErrorMessage = "Campo Obrigatório!")]
        public DateTime DataFinalVigencia { get; set; }

        [Display(Name = "Objeto Resumido")]
        public string ObjetoResumido { get; set; }

        public bool Publicada { get; set; }

        public DateTime DataPublicacaoSistema { get; set; }

        [Display(Name = "Observação")]
        public string Observacao { get; set; }

        public DateTime DataCadastro { get; set; }

        public DateTime DataAlteracao { get; set; }

        public int LimiteAdesao { get; set; }

        public ICollection<ItemViewModel> ItensViewModel { get; set; }
    }
}
