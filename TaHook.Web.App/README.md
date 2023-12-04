## Note for FE development
Visual Studio doesn't really like FE dev with Blazor WASM.
To ensure everything runs properly, instead of Visual Studio build, run:
```
dotnet watch -lp "IIS Express"
```
in one terminal window and
```
npx --yes tailwindcss -i ./Shared/tailwind.css -o ./wwwroot/css/tailwind.css --watch
```
in another. Both commands have to be run from the Web App project root (i.e. the same folder as this readme).

A browser window with hot-reloading of razor pages and automatic Tailwind build will open.