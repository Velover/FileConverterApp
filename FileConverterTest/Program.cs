using FileConverterCore;

//GlobalFFOptions.Configure(options => options.BinaryFolder = @"ffmpeg_7.1\bin");

//Directory.CreateDirectory("Audio");
//Directory.CreateDirectory("Video");
//FFMpegArguments
//	.FromFileInput(@"C:\Users\cobau\Desktop\Resources\Audio\audio.mp3")
//	.OutputToFile(@"Audio\audio.wav")
//	.ProcessSynchronously(true);
//FFMpegArguments
//	.FromFileInput(@"C:\Users\cobau\Desktop\Resources\Video\VideoYoutube.mp4")
//	.OutputToFile(@"Video\audio.mov", true, options => options.WithVideoCodec(VideoCodec.LibX264))
//	.ProcessSynchronously(true);


//FFMpegArguments
//.FromFileInput(@"C:\Users\cobau\Desktop\Resources\Video\VideoYoutube.mp4")
//.OutputToFile(@"Video\audio.mov", true, options => options.WithVideoCodec(VideoCodec.LibX264))
//.ProcessSynchronously(true);



//Thread.Sleep(5000);
//FFMpegArguments.FromFileInput().OutputToFile("audio.wav").ProcessSynchronously();

var txt_path = @"C:\Users\cobau\Desktop\Resources\Text\Txt.txt";
var no_extension_path = @"C:\Users\cobau\Desktop\Resources\Text\Txt";
var audio_path = @"C:\Users\cobau\Desktop\Resources\Audio\audio.mp3";
var video_path = @"C:\Users\cobau\Desktop\Resources\Video\VideoYoutube.mp4";
var image_path = @"C:\Users\cobau\Desktop\Resources\Images\2x2Image.png";

void LogTest(string path, string message)
{
	Console.WriteLine($"Type: {FileConverter.GetFileType(path).ToString()} Message: {message}");
}

//LogTest(txt_path, "Text");
//LogTest(no_extension_path, "No extension");
//LogTest(audio_path, "Audio");
//LogTest(video_path, "Video");
//LogTest(image_path, "Image");

FileConverter.OnFileStatusChanged += (object sender, FileStatusEventArgs args) =>
{
	Console.WriteLine($"Status: {args.ConvertionStatus.ToString()} Message: {args.Message}");
};
FileConverter.ConvertFileToFormat(txt_path, EFileType.Image, "jpg");