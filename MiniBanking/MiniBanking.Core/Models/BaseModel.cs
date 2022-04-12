using Newtonsoft.Json;

namespace MiniBanking.Core.Models;

public abstract class BaseModel
{
    private static readonly JsonSerializerSettings Settings = new JsonSerializerSettings
    {
        Formatting = Formatting.Indented,
        NullValueHandling = NullValueHandling.Ignore,
        ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };
        
    public override string ToString() 
    {
        try
        {
            return JsonConvert.SerializeObject(this, Settings);
        }
        catch (System.Exception)
        {
            //Ignore
        }

        return null;
    }
}