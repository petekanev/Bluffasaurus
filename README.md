# Bluffasaurus Poker Bot

<br/>

## Poker Specific Algorithms

 ###  EHS (Effective Hand Strength)
 ___
 The algorithm is a numerical approach to quantify the strength of a poker hand where its result expresses the strength of a particular hand in percentile (i.e. ranging from 0 to 1), compared to all other possible hands. The underlying assumption is that an Effective Hand Strength (EHS) is composed of the current Hand Strength (HS) and its potential to improve or deteriorate (PPOT and NPOT):
###### EHS = HS \times (1 - NPOT) + (1-HS) \times PPOT
where:
* EHS is the Effective Hand Strength
* HS is the current Hand Strength (i.e. not taking into account potential to improve or deteriorate, depending on upcoming table cards
* NPOT is the Negative POTential (i.e. the probability that our current hand, if the strongest, deteriorates and becomes a losing hand)
* PPOT is the Positive POTential (i.e. the probability that our current hand, if losing, improves and becomes the winning hand

[*Go to top* ^](#Bluffasaurus)<br/>
[*More about the algorithm*](https://en.wikipedia.org/wiki/Poker_Effective_Hand_Strength_(EHS)_algorithm)

 ### Monte Carlo 
___

Using the EHS algorithm, all possible remaining card sequences are
considered. For this reason, this process can be time consuming, given the large
number of possible sequences of cards. For example, the number of
remaining cards is as follows:
* Flop: 
![equation](http://www.sciweavers.org/tex2img.php?eq=%20%5Cfrac%7B47%21%7D%7B%2847-3%29%21%7D%20&bc=White&fc=Black&im=jpg&fs=12&ff=arev&edit=0)
<br/>
<br/>
* Turn:![equation](http://www.sciweavers.org/tex2img.php?eq=%20%5Cfrac%7B46%21%7D%7B%2846-2%29%21%7D%20&bc=White&fc=Black&im=jpg&fs=12&ff=arev&edit=0)

 This would take a lot of
time to calculate.
The solution for this problem is to use Monte Carlo Method. The Monte Carlo
Method considers a small random subset of card sequences. This means, that the
calculation is much faster. The only problem with this technique is that the obtained
result is an approximation. However, our experience showed that the error is very
small, making this a very reliable solution.

[*Go to top* ^](#Bluffasaurus)<br/>
[*More about the algorithm*](http://paginas.fe.up.pt/~niadr/PUBLICATIONS/LIACC_publications_2011_12/pdf/CN10_Estimating_Probability_Winning_LFT.pdf)
<br/>
More information of the usage within the Bot's logic you can find in the <b>Turn</b> section
<br/>
<br/><br/>
## Strategy
 ### Pre-Flop
  * Hand evaluation: using Slansky hand groups. ( [*More information*](https://en.wikipedia.org/wiki/Texas_hold_%27em_starting_hands) )
  * Betting: Based on 
  * hand evalutaion
  * position on the table (i.e if the bot is on small or big blind)
  * amount to call (converted in ![equation](http://www.sciweavers.org/tex2img.php?eq=%20f%28blind%29&bc=White&fc=Black&im=jpg&fs=12&ff=arev&edit=0) )
  
   
Reise | Check / Call
------------ | -------------
![equation](http://www.sciweavers.org/tex2img.php?eq=%20f%28blind%29&bc=White&fc=Black&im=jpg&fs=12&ff=arev&edit=0) | ![equation](http://www.sciweavers.org/tex2img.php?eq=%20f%28blind%2cmoneyToCall%2cleftMoney%29&bc=White&fc=Black&im=jpg&fs=12&ff=arev&edit=0)
 Popular strategy within the pre-flop is <br/>to bet from <b>3x</b> to <b>4x</b> the SB. <br/>Bluffasaurus Poker Bot is reising <br/>in the range <b>6x</b> up to <b>20x</b>. | Playing 75%-85% of the cards <br/>depending on the position on the table.

[*Go to top* ^](#Bluffasaurus)<br/>
___
### Flop
  * Hand evaluation: <br/>
   
Time Limit: 0.1 s | Time Limit 0.05s
------------ | -------------
Using EHS + Monte Carlo with ~ 2000 combinations | using the chart of the (absolute) frequency of each hand. <br/>( [*The chart*](https://en.wikipedia.org/wiki/Poker_probability) )
   
   When using the chart, the AI checks for:
   * Pair
    * Is the pair on the table
    * If not, does the bot holds the highest possible pair for the flop or is it higher than TT
 
 Reise | Check / Call
------------ | -------------
![equation](http://www.sciweavers.org/tex2img.php?eq=%20f%28blind%2cpot%29&bc=White&fc=Black&im=jpg&fs=12&ff=arev&edit=0) | ![equation](http://www.sciweavers.org/tex2img.php?eq=%20f%28blind%2cmoneyToCall%2cleftMoney%29&bc=White&fc=Black&im=jpg&fs=12&ff=arev&edit=0)
Reising in the range <b>8x</b> up to <b>30x</b> the SB. | -
[*Go to top* ^](#Bluffasaurus)<br/>
___
 ### Turn / River
  * Hand evaluation:
  
Turn | River
------------ | -------------
EHS + Monte Carlo | EHS
EHS: ~ 45 K combinations<br/>Monte Carlo: selecting ~ 2 K  | EHS: ~990 combinations

[*Go to top* ^](#Bluffasaurus)<br/>
___


