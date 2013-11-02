// moment.js language configuration
// language : slovenian (sl)
// author : Robert Sedovšek : https://github.com/sedovsek
!function(){function e(e){function n(e,n,t){var a=e+" ";switch(t){case"m":return n?"ena minuta":"eno minuto";case"mm":return a+=1===e?"minuta":2===e?"minuti":3===e||4===e?"minute":"minut";case"h":return n?"ena ura":"eno uro";case"hh":return a+=1===e?"ura":2===e?"uri":3===e||4===e?"ure":"ur";case"dd":return a+=1===e?"dan":"dni";case"MM":return a+=1===e?"mesec":2===e?"meseca":3===e||4===e?"mesece":"mesecev";case"yy":return a+=1===e?"leto":2===e?"leti":3===e||4===e?"leta":"let"}}e.lang("sl",{months:"januar_februar_marec_april_maj_junij_julij_avgust_september_oktober_november_december".split("_"),monthsShort:"jan._feb._mar._apr._maj._jun._jul._avg._sep._okt._nov._dec.".split("_"),weekdays:"nedelja_ponedeljek_torek_sreda_\u010detrtek_petek_sobota".split("_"),weekdaysShort:"ned._pon._tor._sre._\u010det._pet._sob.".split("_"),weekdaysMin:"ne_po_to_sr_\u010de_pe_so".split("_"),longDateFormat:{LT:"H:mm",L:"DD. MM. YYYY",LL:"D. MMMM YYYY",LLL:"D. MMMM YYYY LT",LLLL:"dddd, D. MMMM YYYY LT"},calendar:{sameDay:"[danes ob] LT",nextDay:"[jutri ob] LT",nextWeek:function(){switch(this.day()){case 0:return"[v] [nedeljo] [ob] LT";case 3:return"[v] [sredo] [ob] LT";case 6:return"[v] [soboto] [ob] LT";case 1:case 2:case 4:case 5:return"[v] dddd [ob] LT"}},lastDay:"[v\u010deraj ob] LT",lastWeek:function(){switch(this.day()){case 0:case 3:case 6:return"[prej\u0161nja] dddd [ob] LT";case 1:case 2:case 4:case 5:return"[prej\u0161nji] dddd [ob] LT"}},sameElse:"L"},relativeTime:{future:"\u010dez %s",past:"%s nazaj",s:"nekaj sekund",m:n,mm:n,h:n,hh:n,d:"en dan",dd:n,M:"en mesec",MM:n,y:"eno leto",yy:n},ordinal:"%d.",week:{dow:1,doy:7}})}"function"==typeof define&&define.amd&&define(["moment"],e),"undefined"!=typeof window&&window.moment&&e(window.moment)}();