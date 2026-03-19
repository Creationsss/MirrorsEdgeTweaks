using System;
using System.IO;

namespace MirrorsEdgeTweaks.Helpers
{
    public static class DocumentsPathHelper
    {
        private const string MirrorsEdgeAppId = "17410";

        public static string GetMirrorsEdgeTdGameDocumentsPath(string? gameDirectoryPath)
        {
            string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
            string standardPath = Path.Combine(documentsPath, "EA Games", "Mirror's Edge", "TdGame");

            if (Directory.Exists(Path.Combine(standardPath, "Config")))
            {
                return standardPath;
            }

            string? protonPath = TryGetProtonDocumentsPath(gameDirectoryPath);
            if (protonPath != null)
            {
                return protonPath;
            }

            return standardPath;
        }

        private static string? TryGetProtonDocumentsPath(string? gameDirectoryPath)
        {
            if (!string.IsNullOrEmpty(gameDirectoryPath))
            {
                string? steamappsPath = FindSteamappsParent(gameDirectoryPath);
                if (steamappsPath != null)
                {
                    string candidatePath = Path.Combine(
                        steamappsPath,
                        "compatdata",
                        MirrorsEdgeAppId,
                        "pfx", "drive_c", "users", "steamuser", "Documents",
                        "EA Games", "Mirror's Edge", "TdGame");

                    if (Directory.Exists(candidatePath))
                    {
                        return candidatePath;
                    }
                }
            }

            string homeDir = Environment.GetFolderPath(Environment.SpecialFolder.UserProfile);
            string[] steamLibraryPaths = new[]
            {
                Path.Combine(homeDir, ".local", "share", "Steam", "steamapps"),
                Path.Combine(homeDir, ".steam", "steam", "steamapps"),
                Path.Combine(homeDir, ".steam", "root", "steamapps"),
            };

            foreach (string steamapps in steamLibraryPaths)
            {
                string candidatePath = Path.Combine(
                    steamapps,
                    "compatdata",
                    MirrorsEdgeAppId,
                    "pfx", "drive_c", "users", "steamuser", "Documents",
                    "EA Games", "Mirror's Edge", "TdGame");

                if (Directory.Exists(candidatePath))
                {
                    return candidatePath;
                }
            }

            return null;
        }

        private static string? FindSteamappsParent(string path)
        {
            string? current = path;
            while (!string.IsNullOrEmpty(current))
            {
                string dirName = Path.GetFileName(current);
                if (dirName.Equals("steamapps", StringComparison.OrdinalIgnoreCase))
                {
                    return current;
                }

                string? parent = Path.GetDirectoryName(current);
                if (parent == current)
                {
                    break;
                }

                current = parent;
            }

            return null;
        }
    }
}
