// Town 1
// Gas Attendant PRE PUZZLE

Beau: This damn thing. #bored
Beau: It never lasts more than a week. #bored

* [Rhodes: Oh, should we go somewhere else?] -> DontGo
* [Rhodes: Could I take a look?] -> FixIt
* [Rhodes: Bet I could fix it?] -> FixIt
* [Rhodes: Huh. What’s the issue?] -> FixIt
// goes to FixIt

== DontGo ==
Beau: Sure, but I doubt you’ll find much in this neck of the woods. #neutral
Beau: Quite a few miles to the next town. #neutral
Beau: Can’t even get a repair man in to fix it. #neutral

* [Rhodes: I guess I can give it a try.] -> FixIt
* [Rhodes: Let me give it a try!] -> FixIt
* [Rhodes: Welp, that’s what ya get for relying on the system. I used to work construction, I could give it a go.] -> END
* [Rhodes: Well, I’m no mechanic, but standing here complaining won’t fix it.] -> END 

== FixIt ==
Beau: Yea, I don’t know what good it’ll do, but go for it. #neutral
-> END