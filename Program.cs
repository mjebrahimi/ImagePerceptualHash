using CoenM.ImageHash;
using CoenM.ImageHash.HashAlgorithms;
using Shipwreck.Phash;
using Shipwreck.Phash.Bitmaps;
using System.Drawing;

var projectDir = AppContext.BaseDirectory[..AppContext.BaseDirectory.IndexOf("bin")];
var filename1 = Path.Combine(projectDir, "img1.png");
var filename2 = Path.Combine(projectDir, "img2.webp");

#region ImageHash
//https://github.com/coenm/ImageHash
//https://github.com/VerifyTests/Verify.ImageHash
Console.WriteLine("============================== CoenM.ImageSharp.ImageHash ==============================");
var averageHash1 = GetAverageHash(filename1);
var averageHash2 = GetAverageHash(filename2);
var averageSimilarity = CompareHash.Similarity(averageHash1, averageHash2);
Console.WriteLine($"AverageHash Similarity: {averageSimilarity}");

var perceptualHash1 = GetPerceptualHash(filename1);
var perceptualHash2 = GetPerceptualHash(filename2);
var perceptualSimilarity = CompareHash.Similarity(perceptualHash1, perceptualHash2);
Console.WriteLine($"PerceptualHash Similarity: {perceptualSimilarity}");

var differenceHash1 = GetDifferenceHash(filename1);
var differenceHash2 = GetDifferenceHash(filename2);
var differenceSimilarity = CompareHash.Similarity(differenceHash1, differenceHash2);
Console.WriteLine($"DifferenceHash Similarity: {differenceSimilarity}");
#endregion

Console.WriteLine();
Console.WriteLine();

#region PHash
//https://github.com/pgrho/phash
//https://github.com/VerifyTests/Verify.Phash
Console.WriteLine("============================== Shipwreck.Phash ==============================");
var hash1 = ComputeHash(filename1);
var hash2 = ComputeHash(Path.Combine(projectDir, "img11.png"));
var score = ImagePhash.GetCrossCorrelation(hash1, hash2);
Console.WriteLine($"Cross Correlation: {score * 100}");
#endregion

#region Methods
static ulong GetAverageHash(string filename)
{
    var hashAlgorithm = new AverageHash();
    using var stream = File.OpenRead(filename);
    return hashAlgorithm.Hash(stream);
}

static ulong GetDifferenceHash(string filename)
{
    var hashAlgorithm = new DifferenceHash();
    using var stream = File.OpenRead(filename);
    return hashAlgorithm.Hash(stream);
}

static ulong GetPerceptualHash(string filename)
{
    var hashAlgorithm = new PerceptualHash();
    using var stream = File.OpenRead(filename);
    return hashAlgorithm.Hash(stream);
}

static Digest ComputeHash(string filename)
{
    //Windows Only because of using System.Drawing.Common package
    var bitmap = (Bitmap)Image.FromFile(filename);
    return ImagePhash.ComputeDigest(bitmap.ToLuminanceImage());
}
#endregion
