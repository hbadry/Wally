using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Wally.Utility;

public class AudioModule : ModuleBase<ICommandContext>
{
    // Scroll down further for the AudioService.
    // Like, way down
    private readonly AudioService _service;

    // Remember to add an instance of the AudioService
    // to your IServiceCollection when you initialize your bot
    public AudioModule(AudioService service)
    {
        _service = service;
    }

    // You *MUST* mark these commands with 'RunMode.Async'
    // otherwise the bot will not respond until the Task times out.
    [Command("join", RunMode = RunMode.Async)]
    public async Task JoinCmd()
    {
        await _service.JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel);
    }

    // Remember to add preconditions to your commands,
    // this is merely the minimal amount necessary.
    // Adding more commands of your own is also encouraged.
    [Command("leave", RunMode = RunMode.Async)]
    public async Task LeaveCmd()
    {
        await _service.LeaveAudio(Context.Guild);
    }

    [Command("play", RunMode = RunMode.Async)]
    public async Task PlayCmd([Remainder] string searchKeyword)
    {
        await ReplyAsync("Gathering data please wait");
        var music  = await UtilityHelper.SearchYoutube(searchKeyword);
        if (music == null)
        {
            await ReplyAsync("Can't find these audio");
            return;
        }
        string songName = UtilityHelper.SaveMP3("data", music.Url);
        await _service.JoinAudio(Context.Guild, (Context.User as IVoiceState).VoiceChannel);
        await _service.SendAudioAsync(Context.Guild, Context.Channel,songName);
    }
}