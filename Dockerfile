FROM mcr.microsoft.com/dotnet/sdk:7.0 AS build
WORKDIR /source

COPY ./ ./
RUN dotnet restore

RUN dotnet publish example_site/ExampleSite.csproj --runtime alpine-x64 -c Release --self-contained true /p:PublishTrimmed=true -o /app 

# 2nd stage
# FROM mcr.microsoft.com/dotnet/aspnet:6.0
FROM alpine:latest

RUN apk add --no-cache \ 
    openssh libunwind \
    nghttp2-libs libidn krb5-libs libuuid lttng-ust zlib \
    libstdc++ libintl \
    icu

WORKDIR /app
COPY --from=build /app ./

ENTRYPOINT ["./ExampleSite", "--urls", "http://0.0.0.0:80"]
EXPOSE 80/tcp
EXPOSE 443/tcp

ENV SolidNetsEasy__ApiKey="<insert-api-key>"
ENV SolidNetsEasy__CheckoutKey="<insert-checkout-key>"
ENV SolidNetsEasy__CheckoutUrl="http://my.site/checkout.html"