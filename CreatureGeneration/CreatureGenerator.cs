using System.Drawing;
using System.Drawing.Imaging;
using cryptoapi.Data.Models;

namespace cryptoapi.CreatureGeneration;

public class CreatureGenerator
{
    private string creatureType;

    public Creature Generate()
    {
        var rarityDictionary = rollRarityDictionary();
        var layerDictionary = getImagePaths(rarityDictionary);
        var img = drawImage(layerDictionary);
        var imageBase64 = getBase64(img);
        var traits = getTraits(layerDictionary, rarityDictionary);
        var stats = generateStats(creatureType);

        return new Creature(creatureType.ToString(), imageBase64, 1, stats, traits);
    }

    public Creature Fuse(Creature c1, Creature c2)
    {
        if (c1.Name != c2.Name)
            throw new Exception($"Cannot fuse unlike creatures: {c1.Name} is not {c1.Name}");

        var minRarity = getFusedMinRarityDictionary(c1, c2);
        var rarityDictionary = rollRarityDictionary(minRarity);
        //TODO: Get creature type
        var layerDictionary = getImagePaths(rarityDictionary);

        return c1;
    }

    private Dictionary<ImageLayer, Rarity> getFusedMinRarityDictionary(Creature c1, Creature c2)
    {
        var minRarityDictionary = new Dictionary<ImageLayer, Rarity>();
        var availableTraits = c1.Traits.Concat(c2.Traits).DistinctBy(i => i.layer);

        foreach (var trait in availableTraits)
        {
            var c1TraitRarity = c1.Traits.FirstOrDefault(i => i.layer == trait.layer)?.rarity;
            var c2TraitRarity = c2.Traits.FirstOrDefault(i => i.layer == trait.layer)?.rarity;

            var greaterRarity = Math.Max((int)c1TraitRarity, (int)c2TraitRarity);
            minRarityDictionary.Add(trait.layer, (Rarity)greaterRarity);
        }

        return minRarityDictionary;
    }

    //Generate stats for fighting
    private Stats generateStats(string creatureType) =>

        creatureType switch
        {
            "Dragon" => new Stats(100, 3, 2, 3, 4),
            "Wolf" => new Stats(95, 2, 2, 2, 4),
            "Shrimp" => new Stats(90, 1, 4, 2, 3),
            _ => new Stats(100, 1, 2, 3, 4)
        };


    //Create attributes object from chosen layers
    private IEnumerable<Trait> getTraits(Dictionary<ImageLayer, string> imageLayers, Dictionary<ImageLayer, Rarity> rarityLayers)
    {
        foreach (ImageLayer i in Enum.GetValues(typeof(ImageLayer)))
        {
            if (rarityLayers.TryGetValue(i, out var rarity) && i != ImageLayer.Creatures)
                yield return new Trait(ImageLayer.Backgrounds, rarity, getNameOfPathItem(imageLayers[i]));
        }
    }

    //Bitmap to base64 string
    private string getBase64(Bitmap img)
    {
        using MemoryStream ms = new MemoryStream();
        img.Save(ms, ImageFormat.Png);
        byte[] byteImage = ms.ToArray();
        return Convert.ToBase64String(byteImage);
    }

    //Draw layers to single image
    private Bitmap drawImage(Dictionary<ImageLayer, string> layers)
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
    private Dictionary<ImageLayer, string> getImagePaths(Dictionary<ImageLayer, Rarity> rarityLayers)
    {
        var layerPaths = new Dictionary<ImageLayer, string>();
        //TODO: Look into better approach
        var dir = $"{Directory.GetCurrentDirectory()}\\CreatureGeneration";
        var creaturePath = string.Empty;

        foreach (var layer in rarityLayers)
        {
            var path = string.Empty;
            if (layer.Key == ImageLayer.Creatures)
            {
                path = $"{dir}\\{layer.Key}\\{layer.Value}";
            }
            else if (layer.Key == ImageLayer.Backgrounds ||
            layer.Key == ImageLayer.Foregrounds ||
            layer.Key == ImageLayer.Additional)
            {
                path = $"{dir}\\{layer.Key}\\{layer.Value}";
            }
            else
            {
                path = $"{creaturePath}\\{layer.Key}\\{layer.Value}";
            }

            Console.WriteLine($"path: {path}");

            var choices = layer.Key == ImageLayer.Creatures ?
            Directory.GetDirectories(path) :
            Directory.GetFiles(path);

            var chosenPath = choices[new Random().Next(0, choices.Count())];

            if (layer.Key == ImageLayer.Creatures)
            {
                creaturePath = chosenPath;
                creatureType = getNameOfPathItem(creaturePath);
            }
            else
                layerPaths.Add(layer.Key, chosenPath);
        }
        return layerPaths;
    }

    //Assign layers a rarity
    private Dictionary<ImageLayer, Rarity> rollRarityDictionary(Dictionary<ImageLayer, Rarity> minRarityDictionary = null)
    {
        var rarityDictionary = new Dictionary<ImageLayer, Rarity>();

        if (minRarityDictionary == null)
            rarityDictionary.Add(ImageLayer.Creatures, AttemptRarityRoll());

        var backgroundRarityMin = getRarityFromDictionary(minRarityDictionary, ImageLayer.Backgrounds);
        rarityDictionary.Add(ImageLayer.Backgrounds, AttemptRarityRoll(backgroundRarityMin));

        var bodyRarityMin = getRarityFromDictionary(minRarityDictionary, ImageLayer.Body);
        rarityDictionary.Add(ImageLayer.Body, AttemptRarityRoll(bodyRarityMin));

        var headRarityMin = getRarityFromDictionary(minRarityDictionary, ImageLayer.Head);
        rarityDictionary.Add(ImageLayer.Head, AttemptRarityRoll(headRarityMin));

        var armsLegsRarityMin = getRarityFromDictionary(minRarityDictionary, ImageLayer.ArmsLegs);
        rarityDictionary.Add(ImageLayer.ArmsLegs, AttemptRarityRoll(armsLegsRarityMin));

        var rand = new Random();

        //1 in 10 chance to add a foreground/additional
        var foregroundRarityMin = getRarityFromDictionary(minRarityDictionary, ImageLayer.Foregrounds);
        if (foregroundRarityMin != Rarity.None || rand.Next(0, 10) == 1)
        {
            rarityDictionary.Add(ImageLayer.Foregrounds, AttemptRarityRoll());
        }

        var additionalRarityMin = getRarityFromDictionary(minRarityDictionary, ImageLayer.Additional);
        if (additionalRarityMin != Rarity.None || rand.Next(0, 10) == 1)
        {
            rarityDictionary.Add(ImageLayer.Additional, AttemptRarityRoll());
        }

        return rarityDictionary;
    }

    //Roll for rarity
    private Rarity AttemptRarityRoll(Rarity rarityMin = Rarity.Common)
    {
        var randomRarity = new Random().Next(1, 101) switch
        {
            < 60 => Rarity.Common,//60%
            < 90 => Rarity.Rare,//30%
            < 98 => Rarity.Epic,//8%
            <= 100 => Rarity.Legendary,//2 %
            _ => Rarity.Common
        };

        return (Rarity)Math.Max((int)rarityMin, (int)randomRarity);
    }

    private string getNameOfPathItem(string pathToItem)
    {
        var startOfNameIndex = pathToItem.LastIndexOf("\\") + 1;
        return pathToItem.Substring(startOfNameIndex, pathToItem.Length - startOfNameIndex);
    }

    private Rarity getRarityFromDictionary(Dictionary<ImageLayer, Rarity> dictionary, ImageLayer layer)
    {
        //TODO: Avoid dictionary nullable
        if (dictionary == null)
            return Rarity.Common;

        dictionary.TryGetValue(layer, out var rarityResult);
        return rarityResult;
    }


}