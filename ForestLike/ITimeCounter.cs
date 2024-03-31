using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForestLike;

public interface ITimeCounter
{
    public void StartTime();
    public void SetTheme(string theme);
    public void StopTime();

}
