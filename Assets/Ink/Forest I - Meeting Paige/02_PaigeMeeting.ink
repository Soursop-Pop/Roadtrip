// Meeting Paige for the first time - 3rd Person View

// The camera pulls away from the mini game. You see Paige in third
// person again.

Paige: Whatcha lookin’ at?

* Rhodes: My car broke down again. -> WhatWrong
* Rhodes: Vany -> WhatWrong
* Rhodes: THE ENGINE. -> WhatWrong
* Rhodes: ...  -> WhatWrong

== WhatWrong ==
Paige: What’s wrong with it?

* Rhodes: I have no idea. -> NeedHelp
* Rhodes: Vany -> NeedHelp
* Rhodes: I think it’s the -> NeedHelp
* Rhodes: ... -> NeedHelp

== NeedHelp ==
Paige: Need any help?

* Rhodes: Nah, it’s okay. -> NahOk
* Rhodes: Vany -> VanyResponse
* Rhodes: Do you know anything about cars? -> KnowCars
* Rhodes: ... -> END

== NahOk ==
Paige: Okay, you got this!
-> END

== VanyResponse ==
Paige: That's the spirit!
-> END

== KnowCars ==
Paige: Nope.
Paige: Just look at it like a game!
-> END
