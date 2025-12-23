using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A3.Mercado.Domain.DTOs
{
    public class InstrumentPriceDto
    {
        /// <summary>
        /// Código de Instrumento
        /// </summary>
        public string Code { get; set; }
        /// <summary>
        /// Ultimo precio
        /// </summary>
        public decimal Price { get; set; }
        /// <summary>
        /// variación
        /// </summary>
        public decimal Variation { get; set; }
        /// <summary>
        /// volumen acumulado
        /// </summary>
        public decimal AccumulatedVolume { get; set; }
    }
}
