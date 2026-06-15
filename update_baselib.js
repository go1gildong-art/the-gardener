import fs from "fs";
import fsp from "fs/promises";
import path from "path";
import unzipper from "unzipper";

const repo = "Alchyr/BaseLib-StS2";
const apiUrl = `https://api.github.com/repos/${repo}/releases/latest`;

const destinationFolder = "C:\\Program Files (x86)\\Steam\\steamapps\\common\\Slay the Spire 2\\mods";

const res = await fetch(apiUrl, {
  headers: {
    "User-Agent": "nodejs"
  }
});

if (!res.ok) {
  throw new Error(`GitHub API error: ${res.status}`);
}

const data = await res.json();

// find zip asset
const asset = data.assets.find(a => a.name.endsWith(".zip"));

if (!asset) {
  throw new Error("No zip asset found");
}

const zipPath = path.join(process.env.TEMP, asset.name);

// download file
const fileRes = await fetch(asset.browser_download_url);

if (!fileRes.ok) {
  throw new Error("Download failed");
}

const buffer = Buffer.from(await fileRes.arrayBuffer());
await fsp.writeFile(zipPath, buffer);


// remove old folder
const entries = await fsp.readdir(destinationFolder, { withFileTypes: true });

for (const entry of entries) {
  if (entry.isDirectory() && entry.name.startsWith("BaseLib")) {
    const fullPath = path.join(destinationFolder, entry.name);
    await fsp.rm(fullPath, { recursive: true, force: true });
    console.log("Deleted:", fullPath);
  }
}

// unzip and add baselib to mods folder
const extractedPath = path.join(destinationFolder, asset.name.replace(".zip", ""));
await fsp.mkdir(extractedPath, { recursive: true });
await fs.createReadStream(zipPath)
  .pipe(unzipper.Extract({ path: extractedPath }))
  .promise();

console.log(`Installed ${asset.name}`);