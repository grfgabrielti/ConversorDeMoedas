﻿using ConversorDeMoedas.ACL.Interface;
using ConversorDeMoedas.ACL.Interface.Factory;
using ConversorDeMoedas.Domain;
using ConversorDeMoedas.Domain.Interface;
using ConversorDeMoedas.Domain.Interface.Factory;
using ConversorDeMoedas.Services.Interface;
using ConversorDeMoedas.Services.Request;
using System;
using System.Collections.Generic;

namespace ConversorDeMoedas.Services
{
    public class ConversorService : IConversorService
    {
        IConversorACLFactory conversorACLFactory;
        IMoedaFactory moedaFactory;
        public ConversorService(IConversorACLFactory conversorACLFactory, IMoedaFactory moedaFactory)
        {
            this.conversorACLFactory = conversorACLFactory;
            this.moedaFactory = moedaFactory;
        }

        public List<IMoeda> GetMoedas()
        {
            IConversorACL conversorACL = conversorACLFactory.Create();
            List<IMoeda> retorno = conversorACL.GetMoedas();
            return retorno;
        }

        public IMoeda ConverterMoeda(ConverterMoedaRequest converterMoedaRequest)
        {
            IConversorACL conversorACL = conversorACLFactory.Create();

            IMoeda MoedaOrigem = moedaFactory.Create(converterMoedaRequest.SiglaMoedaOrigem, converterMoedaRequest.ValorParaConversao);
            IMoeda CotacaoEmDolarMoedaOrigem = conversorACL.GetCotacaoComBaseNoDolar(MoedaOrigem.SiglaMoeda);
            IMoeda dinhieroOrigemEmDolar = MoedaOrigem.ConverterParaDolar(CotacaoEmDolarMoedaOrigem);

            IMoeda CotacaoEmDolarMoedaConvertida = conversorACL.GetCotacaoComBaseNoDolar(converterMoedaRequest.MoedaParaConversao);
            Decimal valorDaConversao = dinhieroOrigemEmDolar.ObterValorDaConversaoDeMoeda(CotacaoEmDolarMoedaConvertida);
            IMoeda result = moedaFactory.Create(converterMoedaRequest.MoedaParaConversao, valorDaConversao);

            return result;
        }
    }
}