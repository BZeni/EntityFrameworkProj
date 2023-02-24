using EntityFrameworkProj.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Net.WebSockets;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace EntityFrameworkProj
{
    internal class Methods
    {
        private readonly MarrariDbContext _context;

        public Methods()
        {
            _context = new MarrariDbContext();
        }

        public void Init()
        {
            _context.Database.OpenConnection();        
        }

        public IEnumerable<Lotes> GetLotes(int codProd)
        {
            List<Lotes> lotes = new();

            var query = from l in _context.Lotes
                        from p in _context.Pecas
                        where l.IdLote == p.IdLote && l.CodProd == codProd
                        group l by l.IdLote into newLote
                        select new { newLote.Key, QntPecas = newLote.Count() };

            foreach (var lote in query)
            {
                lotes.Add(new Lotes
                {
                    IdLote = lote.Key,
                    CodProd = lote.QntPecas,
                });
            }

            return lotes;
        }

        public Lotes? GetLote(int num)
        {
            var querySelect = new Lotes();

            var query = from p in _context.Pecas
                        from l in _context.Lotes
                        where l.IdLote == p.IdLote && l.IdLote == num
                        select new { l.IdLote };

            var lotes = query.ToList();

            foreach (var lote in lotes)
            {
                querySelect = new Lotes
                {
                    IdLote = lote.IdLote,
                    CodProd = query.Count(p => p.IdLote == p.IdLote)
                };
            }

            return querySelect;
        }

        public void AddLote(Lotes lote)
        {
            var lotes = new Lotes() { IdLote = lote.IdLote, CodProd = lote.CodProd, Descricao = lote.Descricao };
            _context.Lotes.Add(lotes);
            _context.SaveChanges();

            foreach (var peca in lote.Pecas)
            {
                var pecas = new Pecas(lote.IdLote, peca.Altura, peca.Largura, peca.Comprimento);
                _context.Pecas.Add(pecas);
                _context.SaveChanges();
            }
        }

        public Pecas GetMediaLote(int num)
        {
            var query = from p in _context.Pecas
                        from l in _context.Lotes
                        where l.IdLote == p.IdLote && l.IdLote == num
                        select new { p.IdLote, p.Altura, p.Largura, p.Comprimento };

            var querySelect = new Pecas
            (
                query.Count(p => p.IdLote == p.IdLote),
                query.Average(p => p.Altura),
                query.Average(p => p.Largura),
                query.Average(p => p.Comprimento)
            );

            return querySelect;
        }

        public Pecas GetMediaProduto(int codProd)
        {
            var query = from p in _context.Pecas
                        from l in _context.Lotes
                        where l.IdLote == p.IdLote && l.CodProd == codProd
                        select new { p.IdLote, p.Altura, p.Largura, p.Comprimento };

            var querySelect = new Pecas
            (
                query.Count(p => p.IdLote == p.IdLote),
                query.Average(p => p.Altura),
                query.Average(p => p.Largura),
                query.Average(p => p.Comprimento)
            );

            return querySelect;
        }
    }
}
