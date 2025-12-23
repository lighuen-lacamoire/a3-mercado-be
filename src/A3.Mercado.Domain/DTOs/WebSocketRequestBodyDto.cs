using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace A3.Mercado.Domain.DTOs
{
    public class WebSocketRequestBodyDto<T>
    {
        public string Event { get; set; }
        public T Payload { get; set; }
    }
}
