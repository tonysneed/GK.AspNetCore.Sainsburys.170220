# Build the image:
# docker build -t aspnetcore-docker .

# Create and run a container:
# docker run -d -p 5000:5000 --name aspnetcore-docker aspnetcore-docker

# Linux:
FROM microsoft/dotnet:1.1.0-sdk-projectjson

# Windows Nano Server
# FROM microsoft/dotnet:1.1.0-sdk-projectjson-nanoserver

COPY . /app

WORKDIR /app

RUN ["dotnet", "restore"]

RUN ["dotnet", "build"]

EXPOSE 5000/tcp

ENV ASPNETCORE_ENVIRONMENT=Staging

# Remove this line before checking the file into source code control
ENV MySecret=ProdSecret

CMD ["dotnet", "run", "--server.urls", "http://*:5000"]
