// Meeting Paige for the first time - 3rd Person View

// The camera pulls away from the mini game. You see Paige in third
// person again.

Paige: Whatcha lookin’ at?

* [Rhodes: My car broke down again.] -> WhatWrong
* [Rhodes: Ugh, this car is so old.]  -> WhatWrong
* [Rhodes: THE ENGINE.] -> WhatWrong
* [Rhodes: That…  is a whole mess.] -> WhatWrong

== WhatWrong ==
Paige: What’s wrong with it?

* [Rhodes: I have no idea.] -> NeedHelp
* [Rhodes: The engine crapped out again.] -> NeedHelp
* [Rhodes: I think it’s the flux capacitor.] -> NeedHelp
* [Rhodes: It’s broken.] -> NeedHelp

== NeedHelp ==
Paige: Need any help?

* [Rhodes: Nah, it’s okay.] -> NahOk
* [Rhodes: Yes please! I have no idea what I’m doing.] -> PlsHelp
* [Rhodes: Do you know anything about cars?] -> KnowCars
* [Rhodes: If it’s not “turn it off and on again”, I’m out of ideas.] -> PlsHelp

== NahOk ==
Paige: Okay, you got this!
-> END

== KnowCars ==
Paige: Nope.
Paige: Just look at it like a game!
-> END

== PlsHelp ==
Paige: I don’t know anything about cars, but let’s see!
Paige: Try and think of it as a game! 
-> END