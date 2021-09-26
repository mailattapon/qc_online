FROM microsoft/dotnet:2.2-sdk as build-image

WORKDIR /home/app

COPY ./QcSupplier.csproj ./
COPY ./libman.json ./

RUN dotnet tool install -g Microsoft.Web.LibraryManager.Cli

ENV PATH="$PATH:/root/.dotnet/tools"
ENV ASPNETCORE_ENVIRONMENT="Production"

RUN dotnet restore
RUN libman restore

COPY . .

RUN dotnet publish --output /publish/ --configuration release

FROM mcr.microsoft.com/dotnet/core/aspnet:2.2

WORKDIR /publish

COPY --from=build-image /publish .

ENTRYPOINT ["dotnet", "QcSupplier.dll"]
