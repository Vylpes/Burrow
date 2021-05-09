# Contributing to Burrow

First off, thanks for taking the time to contribute!

The following is a set of guidelines for contributing to Burrow. These are mostly guidelines, not rules. Use your best judgement, and feel free to propose changes to this document in a pull request.

## Code of Conduct

This project and everyone participating in it is governed by the Burrow Code of Conduct. By participating, you are expected to uphold this code.

## Questions about Burrow

> **Note:** Please don't file an issue to ask a question. You'll get faster results by using the resources below.

You can ask a question about the project in the `#development` channel in the [Discord Server](https://discord.gg/UyAhAVp).

You can also ask questions and submit ideas in our [discussions board](https://github.com/Vylpes/Burrow/discussions).

## What you should know

### Typescript and Node

Burrow uses [NodeJS](https://nodejs.org/), as well as TypeScript. You should know how to use this.

## Conventions

There are a few conventions that have developed over time for this project. When you create a pull request a check will be ran making sure that your code follows these conventions.

We won't accept pull requests unless these checks pass. If yours fail, simply fix what the bot says until it passes and then get a repo member to review your code.

* Variable names should use **Camel Case**
* Functions should put **braces on the same line**
* **No comma dangle**, i.e. having a commma after the last item in an object
* **Arrow body style** should have braces around the body only when needed
* **Arrow parameters** should have brackets around them only when needed
* **Arrow spacing** should have a space around the arrow (' => ')
* **No var** should be used, instead use either let or const when appropriate

## How You Can Contribute

### Reporting Bugs

This section guides you through submitting a bug report for Burrow. Following these guidelines helps maintainers and the community understand your report. reproduce the behaviour, and find related reports.

When you are creating a bug report, please include as many details as possible.

> **Note:** If you find a **Closed** issue that seems like it is the same thing that you're experiencing, open a new issue and include a link to the original issue in the body of your new one.

#### Before Submitting A Bug Report

* **Perform a search** to see if the problem has already been reported. If it has **and the issue is still open**, add a comment to the existing issue instead of opening a new one.

#### How You Can Submit A (Good) Bug Report

Bugs are tracked as GitHub issues. After you've determined the bug you're reporting hasn't got a pre-existing **open** issue already, create an issue and provide information from below.

* **Use a clear and descriptive title** for the issue to indentify the problem.
* **Describe the exact steps which reproduce the problem** in as many details as possible. For example, start by explaining how you started Burrow (if you're using your own instance), which command exactly you used, and the output which the bot replied with. If its your own instance, provide information on what the terminal output said, if any.
* **Provide specific examples to demonstrate the steps**. Include links to files or GitHub projects, or copy/pastable snippets, which you use in those examples. If you're providing snippets in the issue, use Markdown code blocks.
* **Describe the behaviour you observed after following the steps** and point out what exactly is the problem with that behaviour.
* **Explain which behaviour you expected to see instead and why.**
* **Include screenshots and animated GIFs** which show you following the described steps and clearly demonstrate the problem.
* **If the problem wasn't triggered by a specific action**, describe what you were doing before the problem happened and share more information using the guidelines below.

Provide more context by answering these questions:

* **Did the problem start happening recently** (e.g. after updating to a new version of Burrow) or was this always a problem?
* If the problem started happening recently, **can you reproduce the problem in an older version of Burrow?** What's the most recently version in which the problem doesn't happen? You can download older versions of Burrow from the releases page.
* **Can you reliably reproduce the issue?** If not, provide details about how often the problem happens and under which conditions it normally happens.

Include details about your configuration and environment:

* **Which version of Burrow are you using?** You can get the exact version by running the `about` command.
* **Do you have any custom commands added to the commands folder?** If you do, provide information on what the command does, and a link to the command file if it exists.

> **Note:** We do not provide any support on issues caused by custom commands. You are welcome to create an issue if you believe the issue is to do with the base code, but if the error is to do with that custom command we are unable to fix that, you will need to contact the author of that command.

* **What's the name and version of the OS you're using?**
* **Are you running Burrow in a virtual machine?** If so, which VM software are you using and which operating systems and versions are used for the host and the guest?
* **What version of node do you have installed?** You can get this version by running the `node -v` command in your terminal.
* **What does your `config.json` file look like?**

> **Note:** remember to **not** give out your bot tokens which are inside of the `config.json` file. If you're giving a copy of your configuration remember to delete the tokens from the string and leave the empty.

* **Are you running the bot in live or dev mode?**

### Suggesting Enhancements

This section guides you through submitting an enhancement suggestion for Burrow, including completely new features and minor improvements to existing functionality. Following these guidelines helps maintainers and the community understand your suggestion and find related suggestions.

When you are creating an enhancement suggestion, please include as many details as possible. Fill out the suggestion with the steps that you imagine you would take if the feature you're requesting existed.

#### Before Submitting an Enhancement Suggestion

* **Check if the feature already exists.** Make sure to check on the latest version and if you can get the desired behaviour using the config options inside of `config.json`
* **Perform a search** to see if the enhancement has already been suggested. If it has, add a comment to the existing issue instead of opening a new one.

#### How Do I Submit A (Good) Enhancement Suggestion?

Enhancement suggestions are tracked as GitHub issues. After you've determined the feature doesn't already exist or been suggested before, create an issue on that repository and provide the following information:

* **Use a clear and descriptive title** for the issue to identify the suggestion.
* **Provide a step-by-step description of the suggested enhancement** in as many details as possible.
* **Provide specific examples to demonstrate the steps.** Include copy/pastable snippets which you use in those examples, as Markdown code blocks.
* **Describe the current behaviour** and **explain which behaviour you expected to see instead** and why.
* **Include screnshots and animated GIFs** which help you demonstrate the steps or point out the part of Burrow which the suggestion is related to.
* **Explain why this enhancement would be useful** to most Burrow users and isn't something that can or should be implemented as a custom command.
* **List some other bots where this enhancement exists.**
* **Specify which version of Burrow you're using.** You can get the exact version by running the `about` command.
* **Specify the name and version of the OS you're using.**

### Your First Code Contribution

Unsure where to begin contributing to Burrow? You can start by looking through these `good first` and `help wanted` issues:

* [Good first issue](https://github.com/vylpes/Burrow/labels/good%20first%20issue) - issues which should only require a few lines of code, and a test or two.
* [Help wanted](https://github.com/vylpes/Burrow/labels/help%20wanted) - issues which should be a bit more involved than `good first` issues.

#### Prerequisites

In order to download necessary tools, clone the repository, and install dependencies via `yarn` you need network access.

You'll need the following tools:

* Git
* NodeJS

Install and build all of the dependencies using `yarn`

```bash
cd Burrow
yarn install
```

#### Build and Run

*Section will be added later*

#### Pull Requests

The process described here has several goals:

* Maintain Burrow's quality
* Fix problems that are important to users
* Engage the community in working toward the best possible Burrow
* Enable a sustainable system for Burrow's maintainers to review contributions

Please follow these steps to have your contribution considered by maintainers:

* You mention the issue id which this pull request aims to fix
* After you submit your pull request, verify that all status checks are passing.

> **Note**: If a check fails the pull request it is important that you go and fix these issues, or let us know that you no longer want to work on this issue by commenting on the pull request. Doing this will give you a better chance of having your pull request merged.

* When the checks have passed a maintainer will review your code and ask for any improvements or questions, and will merge it if they are satisifed.

While the prerequesites above must be satisifed prior to having your pull reuqest accepted, the reviewer(s) may ask you to complete additional design ork, tests, or other changes before your pull request can be ultimately accepted.

#### JavaScript Styleguide

All TypeScript code is linted with `tslint`.

* Prefer camelcase for variable names
* Prefer braces `{` to be on the same line
* Prefer no comma `,` dangle
* Prefer arrow function bodies to have brances `{}` only when needed
* Prefer arrow function parameters to have brackets `()` only when needed
* Prefer arrow function arrows `=>` to have a space before and after it
* Prefer `let` and `const` over `var`

As well as eslint's recommended defaults.

# Contributing to Burrow

First off, thanks for taking the time to contribute!

The following is a set of guidelines for contributing to Burrow. These are mostly guidelines, not rules. Use your best judgement, and feel free to propose changes to this document in a pull request.

## Code of Conduct

This project and everyone participating in it is governed by the Burrow Code of Conduct. By participating, you are expected to uphold this code.

## Questions about Burrow

> **Note:** Please don't file an issue to ask a question. You'll get faster results by using the resources below.

You can ask a question about the project in the `#development` channel in the [Discord Server](https://discord.gg/UyAhAVp).

## What you should know

### TypeScript and Node

Burrow uses [NodeJS](https://nodejs.org/), as well as TypeScript. You should know how to use this.

## Conventions

There are a few conventions that have developed over time for this project. When you create a pull request a check will be ran making sure that your code follows these conventions.

We won't accept pull requests unless these checks pass. If yours fail, simply fix what the bot says until it passes and then get a repo member to review your code.

* Variable names should use **Camel Case**
* Functions should put **braces on the same line**
* **No comma dangle**, i.e. having a commma after the last item in an object
* **Arrow body style** should have braces around the body only when needed
* **Arrow parameters** should have brackets around them only when needed
* **Arrow spacing** should have a space around the arrow (' => ')
* **No var** should be used, instead use either let or const when appropriate

## How You Can Contribute

### Reporting Bugs

This section guides you through submitting a bug report for Burrow. Following these guidelines helps maintainers and the community understand your report. reproduce the behaviour, and find related reports.

When you are creating a bug report, please include as many details as possible.

> **Note:** If you find a **Closed** issue that seems like it is the same thing that you're experiencing, open a new issue and include a link to the original issue in the body of your new one.

#### Before Submitting A Bug Report

* **Perform a search** to see if the problem has already been reported. If it has **and the issue is still open**, add a comment to the existing issue instead of opening a new one.

#### How You Can Submit A (Good) Bug Report

Bugs are tracked as GitHub issues. After you've determined the bug you're reporting hasn't got a pre-existing **open** issue already, create an issue and provide information from below.

* **Use a clear and descriptive title** for the issue to indentify the problem.
* **Describe the exact steps which reproduce the problem** in as many details as possible. For example, start by explaining how you started Burrow (if you're using your own instance), which command exactly you used, and the output which the bot replied with. If its your own instance, provide information on what the terminal output said, if any.
* **Provide specific examples to demonstrate the steps**. Include links to files or GitHub projects, or copy/pastable snippets, which you use in those examples. If you're providing snippets in the issue, use Markdown code blocks.
* **Describe the behaviour you observed after following the steps** and point out what exactly is the problem with that behaviour.
* **Explain which behaviour you expected to see instead and why.**
* **Include screenshots and animated GIFs** which show you following the described steps and clearly demonstrate the problem.
* **If the problem wasn't triggered by a specific action**, describe what you were doing before the problem happened and share more information using the guidelines below.

Provide more context by answering these questions:

* **Did the problem start happening recently** (e.g. after updating to a new version of Burrow) or was this always a problem?
* If the problem started happening recently, **can you reproduce the problem in an older version of Burrow?** What's the most recently version in which the problem doesn't happen? You can download older versions of Burrow from the releases page.
* **Can you reliably reproduce the issue?** If not, provide details about how often the problem happens and under which conditions it normally happens.

Include details about your configuration and environment:

* **Which version of Burrow are you using?** You can get the exact version by running the `about` command.
* **Do you have any custom commands added to the commands folder?** If you do, provide information on what the command does, and a link to the command file if it exists.

> **Note:** We do not provide any support on issues caused by custom commands. You are welcome to create an issue if you believe the issue is to do with the base code, but if the error is to do with that custom command we are unable to fix that, you will need to contact the author of that command.

* **What's the name and version of the OS you're using?**
* **Are you running Burrow in a virtual machine?** If so, which VM software are you using and which operating systems and versions are used for the host and the guest?
* **What version of node do you have installed?** You can get this version by running the `node -v` command in your terminal.
* **What does your `config.json` file look like?**

> **Note:** remember to **not** give out your bot tokens which are inside of the `config.json` file. If you're giving a copy of your configuration remember to delete the tokens from the string and leave the empty.

* **Are you running the bot in live or dev mode?**

### Suggesting Enhancements

This section guides you through submitting an enhancement suggestion for Burrow, including completely new features and minor improvements to existing functionality. Following these guidelines helps maintainers and the community understand your suggestion and find related suggestions.

When you are creating an enhancement suggestion, please include as many details as possible. Fill out the suggestion with the steps that you imagine you would take if the feature you're requesting existed.

#### Before Submitting an Enhancement Suggestion

* **Check if the feature already exists.** Make sure to check on the latest version and if you can get the desired behaviour using the config options inside of `config.json`
* **Perform a search** to see if the enhancement has already been suggested. If it has, add a comment to the existing issue instead of opening a new one.

#### How Do I Submit A (Good) Enhancement Suggestion?

Enhancement suggestions are tracked as GitHub issues. After you've determined the feature doesn't already exist or been suggested before, create an issue on that repository and provide the following information:

* **Use a clear and descriptive title** for the issue to identify the suggestion.
* **Provide a step-by-step description of the suggested enhancement** in as many details as possible.
* **Provide specific examples to demonstrate the steps.** Include copy/pastable snippets which you use in those examples, as Markdown code blocks.
* **Describe the current behaviour** and **explain which behaviour you expected to see instead** and why.
* **Include screnshots and animated GIFs** which help you demonstrate the steps or point out the part of Burrow which the suggestion is related to.
* **Explain why this enhancement would be useful** to most Burrow users and isn't something that can or should be implemented as a custom command.
* **List some other bots where this enhancement exists.**
* **Specify which version of Burrow you're using.** You can get the exact version by running the `about` command.
* **Specify the name and version of the OS you're using.**

### Your First Code Contribution

Unsure where to begin contributing to Burrow? You can start by looking through these `good first` and `help wanted` issues:

* [Good first issue](https://github.com/vylpes/Burrow/labels/good%20first%20issue) - issues which should only require a few lines of code, and a test or two.
* [Help wanted](https://github.com/vylpes/Burrow/labels/help%20wanted) - issues which should be a bit more involved than `good first` issues.

#### Prerequisites

In order to download necessary tools, clone the repository, and install dependencies via `yarn` you need network access.

You'll need the following tools:

* Git
* NodeJS

Install and build all of the dependencies using `npm`

```bash
cd Burrow
npm install
cp config.json.template config.json
```
And then use your text editor of choice to fill in the `config.json` file.

#### Build and Run

*Section to be added later*

#### Pull Requests

The process described here has several goals:

* Maintain Burrow's quality
* Fix problems that are important to users
* Engage the community in working toward the best possible Burrow
* Enable a sustainable system for Burrow's maintainers to review contributions

Please follow these steps to have your contribution considered by maintainers:

* You mention the issue id which this pull request aims to fix
* After you submit your pull request, verify that all status checks are passing.

> **Note**: If a check fails the pull request it is important that you go and fix these issues, or let us know that you no longer want to work on this issue by commenting on the pull request. Doing this will give you a better chance of having your pull request merged.

* When the checks have passed a maintainer will review your code and ask for any improvements or questions, and will merge it if they are satisifed.

While the prerequesites above must be satisifed prior to having your pull reuqest accepted, the reviewer(s) may ask you to complete additional design ork, tests, or other changes before your pull request can be ultimately accepted.

#### JavaScript Styleguide

All JavaScript code is linted with `eslint`.

* Prefer camelcase for variable names
* Prefer braces `{` to be on the same line
* Prefer no comma `,` dangle
* Prefer arrow function bodies to have brances `{}` only when needed
* Prefer arrow function parameters to have brackets `()` only when needed
* Prefer arrow function arrows `=>` to have a space before and after it
* Prefer `let` and `const` over `var`

As well as eslint's recommended defaults.