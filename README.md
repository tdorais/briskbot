# briskbot
A bot for the [brisk challenge](http://www.briskchallenge.com)

Requirements: [.NET Core 2.2](https://dotnet.microsoft.com/download/dotnet-core/2.2)

*This document will be subject to change*

## Roadmap
* ~~Build Hello World console app~~
* ~~Make call to API~~
  * ~~API: Create Game~~
* ~~Add Unit Tests~~
* ~~Implement pass-turn-till-end gameplay~~
  * ~~API: Check for Turn~~
  * ~~API: End Turn~~
* ~~Error Handling~~
  * ~~Handle faulty server calls~~
* Clean up "config" settings, like urls
* Refine console logging
* Set armies randomly
  * requies mapping
* Attack randomly
* Implement strategies
  * Analysize map
  * Set continent goals
  * determine territory priorities
  * determine army construction
* Implement Tactics
  * Build logic for when to halt attack
  * build army war path
* Implement PvP
  * Create a joinable game
  * Join a game
  * Defeat all my friends
  * Salvage relationships afterwards

## Design Philosophy
* Build only what you need
* Make refactor easy
* Break into vertical slices

## Design Decisions
* .NET Core
  * Chose Core over Framework for cross-platform distribution, since there is no guarantee any of the team reviewing will be using Windows.
    * This has implications as the packages that can be imported are limited, but workarounds were not overly laborious.
* Unit Testing
  * Unit Tests should define what is expected from a function
  * Only build and test what you need
  * Created used decorator pattern over HttpClient for dependency injection and mocking
  * Test names should read well in English and follow this format: {subject} {conditions} {results}
* Async
  * Call async methods from async whenever possible.
  * Only in the main console app will we block and wait for the tasks to resolve, since there is only one thread at the console, it will be deadlocked with any competing resources.
* HttpClient is created in Main and left open to reduce overhead of spinning new clients for each request
  * new HttpClients can take ~35 ms to instantiate.
  * The application life is short enough to not run into problems like invalid DNS registers.
* Error Handling
  * Throwing basic exceptions until a more defined granularity presents itself
  * Leaving exceptions uncaught initially for complete clarity during development.
  * More precise exception handling can be done once the reason presents itself. For now, complete meltdowns work best.

## What I Could Have Done Differently
* Using .NET Framework (and going solution heavy with Visual Studio) could have saved some time
* Some of the unit tests could have been heavy on pre-optimization.

## Bug Report
*AKA: my shame*
* Ctrl+C is not exitting program.