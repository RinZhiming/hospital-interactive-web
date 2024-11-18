using Fusion.Photon.Realtime;
using Photon.Chat;

public class PhotonChatModel
{
     public ChatClient ChatClient { get; set; }
     public FusionAppSettings Setting { get; set; }
     private string ChannelName { get; set; }
}