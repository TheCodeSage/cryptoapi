using System.Drawing;
using System.Drawing.Imaging;

public class CreatureGenerator
{
    private Rarity CurrentRarity;
    private CreatureType creatureType;

    public Creature Generate()
    {
        var rarityLayers = rollRarityDictionary();
        var layerDictionary = getImagePaths(rarityLayers);
        var img = drawImage(layerDictionary);
        var imageBase64 = getBase64(img);

        return new Creature(creatureType.ToString(), imageBase64, 1, generateStats(creatureType), getAttributes(layerDictionary));
    }

    //Generate stats for fighting
    private Stats generateStats(CreatureType creatureType)
    {
        return new Stats(100, 1, 2, 3, 4);
    }

    //Create attributes object from chosen layers
    private Attributes getAttributes(Dictionary<ImageRarityLayer, string> imageLayers) => 
        new Attributes(
            getNameOfPathItem(imageLayers[ImageRarityLayer.Backgrounds]),
            getNameOfPathItem(imageLayers[ImageRarityLayer.Body]),
            getNameOfPathItem(imageLayers[ImageRarityLayer.Head]),
            getNameOfPathItem(imageLayers[ImageRarityLayer.ArmsLegs]))
        {
            Foreground = imageLayers.TryGetValue(ImageRarityLayer.Foregrounds, out var foreground) ? foreground : "",
            Additional = imageLayers.TryGetValue(ImageRarityLayer.Additional, out var additional) ? additional : ""
        };   

    //Bitmap to base64 string
    private string getBase64(Bitmap img)
    {
        using MemoryStream ms = new MemoryStream();
        img.Save(ms, ImageFormat.Png);
        byte[] byteImage = ms.ToArray();
        return Convert.ToBase64String(byteImage);
    }

    //Draw layers to single image
    private Bitmap drawImage(Dictionary<ImageRarityLayer, string> layers)
    {
        var image = new Bitmap(1200, 1200);
        var graphics = Graphics.FromImage(image);
        foreach (var layer in layers) 
        {            
            var layerImage = new Bitmap(layer.Value);
            graphics.DrawImage(layerImage, 0, 0);
        }        
        return image;
    }

    //Get path names to chosen layers
    private Dictionary<ImageRarityLayer, string> getImagePaths(Dictionary<ImageRarityLayer, Rarity> rarityLayers)
    {
        var layerPaths = new Dictionary<ImageRarityLayer, string>();
        //TODO: Look into better approach
        var dir = $"{Directory.GetCurrentDirectory()}\\CreatureGenerator";
        var creaturePath = string.Empty;

        foreach (var layer in rarityLayers)
        {
            var path = string.Empty;
            if (layer.Key == ImageRarityLayer.Creatures)
            {
                path = $"{dir}\\{layer.Key}\\{layer.Value}";
            }
            else if(layer.Key == ImageRarityLayer.Backgrounds ||
            layer.Key == ImageRarityLayer.Foregrounds ||
            layer.Key == ImageRarityLayer.Additional)
            {
                path = $"{dir}\\{layer.Key}\\{layer.Value}";
            }
            else
            {
                path = $"{creaturePath}\\{layer.Key}\\{layer.Value}";
            }

            Console.WriteLine($"path: {path}");

            var choices = layer.Key == ImageRarityLayer.Creatures ?
            Directory.GetDirectories(path) :
            Directory.GetFiles(path);

            var chosenPath = choices[new Random().Next(0, choices.Count())];

            if (layer.Key == ImageRarityLayer.Creatures)
            {
                creaturePath = chosenPath;
                Enum.TryParse(getNameOfPathItem(creaturePath), out creatureType);
            }
            else
                layerPaths.Add(layer.Key, chosenPath);
        }
        return layerPaths;
    }

    //Assign layers a rarity
    private Dictionary<ImageRarityLayer, Rarity> rollRarityDictionary()
    {
        var rarityDictionary = new Dictionary<ImageRarityLayer, Rarity>(){
            {ImageRarityLayer.Creatures, AttemptRarityRoll(false)},
            {ImageRarityLayer.Backgrounds, AttemptRarityRoll()},
            {ImageRarityLayer.Body, AttemptRarityRoll()},
            {ImageRarityLayer.Head, AttemptRarityRoll()},
            {ImageRarityLayer.ArmsLegs, AttemptRarityRoll()},
        };

        var rand = new Random();
        //1 in 10 chance to add a foreground/additional
        if (rand.Next(0, 10) == 1)
            rarityDictionary.Add(ImageRarityLayer.Foregrounds, AttemptRarityRoll());
        if (rand.Next(0, 10) == 1)
            rarityDictionary.Add(ImageRarityLayer.Additional, AttemptRarityRoll());

        return rarityDictionary;
    }

    //Roll for rarity
    private Rarity AttemptRarityRoll(bool persistRarity = true)
    {
        var randomRarity = new Random().Next(1, 101) switch
        {
            < 60 => Rarity.Common,//60%
            < 90 => Rarity.Rare,//30%
            < 98 => Rarity.Epic,//8%
            <= 100 => Rarity.Legendary,//2 %
            _ => Rarity.Common
        };

        var chosenRarity = (Rarity)Math.Max((int)CurrentRarity, (int)randomRarity);

        if (persistRarity)
            CurrentRarity = chosenRarity;

        return chosenRarity;
    }

    private string getNameOfPathItem(string pathToItem)
    {
        var startOfNameIndex = pathToItem.LastIndexOf("\\") + 1;
        return pathToItem.Substring(startOfNameIndex, pathToItem.Length - startOfNameIndex);
    }
}