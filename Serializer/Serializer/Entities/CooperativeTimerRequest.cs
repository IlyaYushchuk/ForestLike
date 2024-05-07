using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Serializer.Entities;

//TODO dont know where this class must be
public class CooperativeTimerRequest
{
    public string SenderName { get; set; }
    public string ReceiverName { get; set; }
    public TimeSpan Time { get; set; }
    public string Theme { get; set; }
    public string Description {  get; set; }

}
