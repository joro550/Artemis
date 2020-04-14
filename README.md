Artemis
> Artemis, in Greek religion, the goddess of wild animals, the hunt, and vegetation, and of chastity and childbirth;

![.NET Core](https://github.com/joro550/Artemis/workflows/.NET%20Core/badge.svg)
![License](https://img.shields.io/github/license/joro550/Artemis)

# What is this?
This is a project that I am going to submit for the dev.to hackathon in association with twilio on 2020-04. 

*Quick intro*

Right now the world has it's mind on COVID-19 how can you help? But looking beyond and forward how do we help the homeless the starving, how do we fix global warming. This project is to help people who want to help with all of these causes and more, oganizations can create events that they are currently running i.e. Donate to fix a curch roof, and have a quick "call to action" that people can use to donate to help that event. 

Updates can be published to events to share how progress is coming along so that people are informed with how their contribution helped with the cause. 

*Twilio integration*

There are a few integration points that I can see - and a few more coming in the future.

**Subscribing to an organization** - This will let you know via sms when an organization has created an event. So each time that organization requires you to help you can choose to do so

**Subscribing to an event** - This will let you know each time an update has been published to an event so that you can keep up to date with your contributioon and the event

**Subscribing to a region** - Allow people to input a postcode and a radius in which they are interested so if *any* event is created within that radius they will get a message so that they will know how to help their local communities.

# Running the website

You will need 
- Visual studio 2019 (most up to date version)
- Dotnet core (at least 3.1 - [download page](https://dotnet.microsoft.com/download))

Potential IDE's
- Visual studio 2019 (most up to date version)
- Project rider (jetbrains)
- Visual studio code

## Logging in 
There is a class called `DataSeeder` in there we create a user currently with the email address `user@email.com` and a password, the data seeder **is only ever ran if it is in development mode** so once the project is deployed to an environment without the environment variable of `ASPNETCORE_DEVELOPMENT=true` then this data seeder will not import any data meaning this user will not exist.

This user should be the creator of all of the organizations etc. on the site that have also been created by the data seeder, this means they should be able to view/edit/delete/add etc. (most of this is to come) mostly so that you don't have to register a new user as that would require you to setup a captcha account.

## Sending messages

This will require you to set up a twilio account and setup the appropriate dotnet secrets (explained later).

**Message templates** - An organization will have with it a list of message templates, these are associated with an event that happens on the website i.e. Event Created

> Basic logic : When an event gets triggered by the website i.e. event created, it will find the message template associated with that particular event, find all of the subscribers to the organization/event/location and then send that message template via twilio. It will then save the message id from twilio into the data store alongside the userid it sent it to.


## Dot net secrets
There are a number of secrets that will need to be set for certain things to work - I'm currently not sure if this is the bes tway to do this but for now I think it is suffucient:

[Click here](https://docs.microsoft.com/en-us/aspnet/core/security/app-secrets?view=aspnetcore-3.1&tabs=windows) for the documentation around dot net secrets

Here are the secrets the application currently uses and what it uses them for:

|Key |Value | Use|
--|--|--
Twilio:AccountSid | The Sid for the twilio account | To integrate with the messaging api
Twilio:FromNumber | The number with which twilio will use to send the message | The number with which twilio will use to send the message
Twilio:Token | Twilio auth token  | The auth token in which to use to authenticate with twilio messaging
UserConfig:PhoneNumber | A phone number you own | Whilst in developer mode the data seeder will add a user with this phone number 
Captcha:SecretKey | Secret key handed out when setting up a reCaptcha | The secret key to send to the reCaptcha api when user registers

