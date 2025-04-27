˜'
Y/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Pulumock/Utilities/OutputMerger.cs
	namespace 	
Pulumock
 
. 
	Utilities 
; 
internal 
static	 
class 
OutputMerger "
{ 
public 

static 
ImmutableDictionary %
<% &
string& ,
,, -
object. 4
>4 5
Merge6 ;
(; <
IReadOnlyDictionary 
< 
string "
," #
object$ *
>* +
baseDict, 4
,4 5
IReadOnlyDictionary		 
<		 
string		 "
,		" #
object		$ *
>		* +
overrideDict		, 8
)		8 9
{

 

Dictionary 
< 
string 
, 
object !
>! "
result# )
=* +
	ToMutable, 5
(5 6
baseDict6 >
)> ?
;? @
foreach 
( 
( 
string 
key 
, 
object $
overrideValue% 2
)2 3
in4 6
overrideDict7 C
)C D
{ 	
if 
( 
result 
. 
TryGetValue "
(" #
key# &
,& '
out( +
object, 2
?2 3
	baseValue4 =
)= >
&&? A
TryAsDictionary 
(  
	baseValue  )
,) *
out+ .

Dictionary/ 9
<9 :
string: @
,@ A
objectB H
>H I

baseNestedJ T
)T U
&&V X
TryAsDictionary 
(  
overrideValue  -
,- .
out/ 2

Dictionary3 =
<= >
string> D
,D E
objectF L
>L M
overrideNestedN \
)\ ]
)] ^
{ 
result 
[ 
key 
] 
= 
Merge #
(# $

baseNested$ .
,. /
overrideNested0 >
)> ?
;? @
} 
else 
{ 
result 
[ 
key 
] 
= 
overrideValue +
;+ ,
} 
} 	
return 
result 
. !
ToImmutableDictionary +
(+ ,
), -
;- .
} 
private 
static 

Dictionary 
< 
string $
,$ %
object& ,
>, -
	ToMutable. 7
(7 8
IReadOnlyDictionary8 K
<K L
stringL R
,R S
objectT Z
>Z [
dict\ `
)` a
=>b d
dict 
. 
ToDictionary 
( 
kvp   
=>   
kvp   
.   
Key   
,   
kvp!! 
=>!! 
TryAsDictionary!! "
(!!" #
kvp!!# &
.!!& '
Value!!' ,
,!!, -
out!!. 1

Dictionary!!2 <
<!!< =
string!!= C
,!!C D
object!!E K
>!!K L
nested!!M S
)!!S T
?"" 
	ToMutable"" 
("" 
nested"" "
)""" #
:## 
kvp## 
.## 
Value## 
)$$ 	
;$$	 

private&& 
static&& 
bool&& 
TryAsDictionary&& '
(&&' (
object&&( .
?&&. /
obj&&0 3
,&&3 4
out&&5 8

Dictionary&&9 C
<&&C D
string&&D J
,&&J K
object&&L R
>&&R S
dict&&T X
)&&X Y
{'' 
switch(( 
((( 
obj(( 
)(( 
{)) 	
case** 
ImmutableDictionary** $
<**$ %
string**% +
,**+ ,
object**- 3
>**3 4
	immutable**5 >
:**> ?
dict++ 
=++ 
	immutable++  
.++  !
ToDictionary++! -
(++- .
kvp++. 1
=>++2 4
kvp++5 8
.++8 9
Key++9 <
,++< =
kvp++> A
=>++B D
kvp++E H
.++H I
Value++I N
)++N O
;++O P
return,, 
true,, 
;,, 
case.. 

Dictionary.. 
<.. 
string.. "
,.." #
object..$ *
>..* +
d.., -
:..- .
dict// 
=// 
d// 
;// 
return00 
true00 
;00 
default22 
:22 
dict33 
=33 
null33 
!33 
;33 
return44 
false44 
;44 
}55 	
}66 
}77 Ï2
W/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Pulumock/Utilities/MockHelper.cs
	namespace 	
Pulumock
 
. 
	Utilities 
; 
public		 
static		 
class		 

MockHelper		 
{

 
public 

static 
bool 
IsStackReference '
(' (
MockResourceArgs( 8
args9 =
)= >
=>? A
string 
. 
Equals 
( 
args 
. 
Type 
,  &
ResourceTypeTokenConstants! ;
.; <
StackReference< J
,J K
StringComparisonL \
.\ ]
Ordinal] d
)d e
;e f
public 

static 
string "
GetLogicalResourceName /
(/ 0
string0 6
?6 7
name8 <
)< =
=>> @
string 
. 
IsNullOrWhiteSpace !
(! "
name" &
)& '
?( )
throw* /
new0 3!
ArgumentNullException4 I
(I J
nameofJ P
(P Q
nameQ U
)U V
)V W
:X Y
nameZ ^
;^ _
public 

static 
object #
GetPhysicalResourceName 0
(0 1
MockResourceArgs1 A
argsB F
,F G
ImmutableDictionaryH [
<[ \
string\ b
,b c
objectd j
>j k
.k l
Builderl s
outputst {
){ |
=>} 
outputs 
. 
GetValueOrDefault !
(! "
$str" (
)( )
??* ,
$"- /
{/ 0"
GetLogicalResourceName0 F
(F G
argsG K
.K L
NameL P
)P Q
}Q R
$strR [
"[ \
;\ ]
public 

static 
string 
GetResourceId &
(& '
string' -
?- .
id/ 1
,1 2
string3 9

fallbackId: D
)D E
=>F H
string 
. 
IsNullOrWhiteSpace !
(! "
id" $
)$ %
?& '

fallbackId( 2
:3 4
id5 7
;7 8
public 

static 
string 
GetCallToken %
(% &
string& ,
?, -
token. 3
)3 4
=>5 7
string 
. 
IsNullOrWhiteSpace !
(! "
token" '
)' (
?) *
throw+ 0
new1 4!
ArgumentNullException5 J
(J K
nameofK Q
(Q R
tokenR W
)W X
)X Y
:Z [
token\ a
;a b
public 

static 
MockResource 
? $
GetMockResourceOrDefault  8
(8 9
ImmutableDictionary 
< 
( 
Type !
Type" &
,& '
string( .
?. /
LogicalName0 ;
); <
,< =
MockResource> J
>J K
	resourcesL U
,U V
string 
? 
	typeToken 
, 
string 
? 
logicalName 
) 
{ 
MockResource 
? 
match 
= 
	resources '
.   
SingleOrDefault   
(   
kvp    
=>  ! #
kvp!! 
.!! 
Key!! 
.!! 
Type!! 
.!! $
MatchesResourceTypeToken!! 5
(!!5 6
	typeToken!!6 ?
)!!? @
&&!!A C
string"" 
."" 
Equals"" 
("" 
kvp"" !
.""! "
Key""" %
.""% &
LogicalName""& 1
,""1 2
logicalName""3 >
,""> ?
StringComparison""@ P
.""P Q
Ordinal""Q X
)""X Y
)""Y Z
.## 
Value## 
;## 
return%% 
match%% 
??%% 
	resources%% !
.&& 
SingleOrDefault&& 
(&& 
kvp&&  
=>&&! #
kvp'' 
.'' 
Key'' 
.'' 
Type'' 
.'' $
MatchesResourceTypeToken'' 5
(''5 6
	typeToken''6 ?
)''? @
&&''A C
kvp(( 
.(( 
Key(( 
.(( 
LogicalName(( #
is(($ &
null((' +
)((+ ,
.)) 
Value)) 
;)) 
}** 
public,, 

static,, 
MockCall,, 
?,,  
GetMockCallOrDefault,, 0
(,,0 1
ImmutableDictionary,,1 D
<,,D E
MockCallToken,,E R
,,,R S
MockCall,,T \
>,,\ ]
calls,,^ c
,,,c d
string,,e k
	callToken,,l u
),,u v
{-- 
MockCall.. 
?.. 
match.. 
=.. 
calls.. 
.// 
FirstOrDefault// 
(// 
kvp// 
=>//  "
kvp00 
.00 
Key00 
.00 
IsStringToken00 %
&&00& (
string11 
.11 
Equals11 
(11 
kvp11 !
.11! "
Key11" %
.11% &
StringTokenValue11& 6
,116 7
	callToken118 A
,11A B
StringComparison11C S
.11S T
Ordinal11T [
)11[ \
)11\ ]
.22 
Value22 
;22 
return44 
match44 
??44 
calls44 
.55 
FirstOrDefault55 
(55 
kvp55 
=>55  "
kvp66 
.66 
Key66 
.66 
IsTypeToken66 #
&&66$ &
kvp77 
.77 
Key77 
.77 
TypeTokenValue77 &
.77& ' 
MatchesCallTypeToken77' ;
(77; <
	callToken77< E
)77E F
)77F G
.88 
Value88 
;88 
}99 
}:: õC
^/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Pulumock/TestFixtures/FixtureBuilder.cs
	namespace 	
Pulumock
 
. 
TestFixtures 
;  
public 
class 
FixtureBuilder 
{ 
private 
MockConfiguration 
? 
_mockConfiguration 1
;1 2
private 
readonly 

Dictionary 
<  
(  !
Type! %
Type& *
,* +
string, 2
?2 3
LogicalName4 ?
)? @
,@ A
MockResourceB N
>N O
_mockResourcesP ^
=_ `
newa d
(d e
)e f
;f g
private 
readonly 

Dictionary 
<  
MockCallToken  -
,- .
MockCall/ 7
>7 8

_mockCalls9 C
=D E
newF I
(I J
)J K
;K L
public 

FixtureBuilder !
WithMockConfiguration /
(/ 0
MockConfiguration0 A
mockConfigurationB S
)S T
{ 
_mockConfiguration 
= 
mockConfiguration .
;. /
return 
this 
; 
} 
public 

FixtureBuilder $
WithoutMockConfiguration 2
(2 3
)3 4
{ 
_mockConfiguration 
= 
null !
;! "
return 
this 
; 
} 
public 

FixtureBuilder "
WithMockStackReference 0
(0 1
MockStackReference1 C
mockStackReferenceD V
)V W
{ 
_mockResources   
[   
(   
mockStackReference   *
.  * +
Type  + /
,  / 0
mockStackReference  1 C
.  C D#
FullyQualifiedStackName  D [
)  [ \
]  \ ]
=  ^ _
mockStackReference  ` r
;  r s
return!! 
this!! 
;!! 
}"" 
public$$ 

FixtureBuilder$$ %
WithoutMockStackReference$$ 3
($$3 4
MockStackReference$$4 F
mockStackReference$$G Y
)$$Y Z
{%% 
_mockResources&& 
.&& 
Remove&& 
(&& 
(&& 
mockStackReference&& 1
.&&1 2
Type&&2 6
,&&6 7
mockStackReference&&8 J
.&&J K#
FullyQualifiedStackName&&K b
)&&b c
)&&c d
;&&d e
return'' 
this'' 
;'' 
}(( 
public** 

FixtureBuilder** 
WithMockResource** *
(*** +
MockResource**+ 7
mockResource**8 D
)**D E
{++ 
_mockResources,, 
[,, 
(,, 
mockResource,, $
.,,$ %
Type,,% )
,,,) *
mockResource,,+ 7
.,,7 8
LogicalName,,8 C
),,C D
],,D E
=,,F G
mockResource,,H T
;,,T U
return-- 
this-- 
;-- 
}.. 
public00 

FixtureBuilder00 
WithoutMockResource00 -
(00- .
MockResource00. :
mockResource00; G
)00G H
{11 
_mockResources22 
.22 
Remove22 
(22 
(22 
mockResource22 +
.22+ ,
Type22, 0
,220 1
mockResource222 >
.22> ?
LogicalName22? J
)22J K
)22K L
;22L M
return33 
this33 
;33 
}44 
public66 

FixtureBuilder66 
WithMockCall66 &
(66& '
MockCall66' /
mockCall660 8
)668 9
{77 
MockCallToken88 
newKey88 
=88 
mockCall88 '
.88' (
Token88( -
;88- .
foreach:: 
(:: 
MockCallToken:: 
existingKey:: *
in::+ -

_mockCalls::. 8
.::8 9
Keys::9 =
.;; 
Where;; 
(;; 
existingKey;; '
=>;;( *
existingKey;;+ 6
.;;6 7
ConflictsWith;;7 D
(;;D E
newKey;;E K
);;K L
);;L M
);;M N
{<< 	

_mockCalls== 
.== 
Remove== 
(== 
existingKey== )
)==) *
;==* +
break>> 
;>> 
}?? 	

_mockCallsAA 
[AA 
newKeyAA 
]AA 
=AA 
mockCallAA %
;AA% &
returnBB 
thisBB 
;BB 
}CC 
publicEE 

FixtureBuilderEE 
WithoutMockCallEE )
(EE) *
MockCallEE* 2
mockCallEE3 ;
)EE; <
{FF 
MockCallTokenGG 
?GG 
existingKeyGG "
=GG# $

_mockCallsGG% /
.GG/ 0
KeysGG0 4
.HH 
FirstOrDefaultHH 
(HH 
kHH 
=>HH  
kHH! "
.HH" #
ConflictsWithHH# 0
(HH0 1
mockCallHH1 9
.HH9 :
TokenHH: ?
)HH? @
)HH@ A
;HHA B
ifJJ 

(JJ 
existingKeyJJ 
isJJ 
notJJ 
nullJJ #
)JJ# $
{KK 	

_mockCallsLL 
.LL 
RemoveLL 
(LL 
existingKeyLL )
)LL) *
;LL* +
}MM 	
returnOO 
thisOO 
;OO 
}PP 
publicRR 

asyncRR 
TaskRR 
<RR 
FixtureRR 
>RR 

BuildAsyncRR )
(RR) *
FuncRR* .
<RR. /
TaskRR/ 3
<RR3 4
IDictionaryRR4 ?
<RR? @
stringRR@ F
,RRF G
objectRRH N
?RRN O
>RRO P
>RRP Q
>RRQ R
createResourcesFuncRRS f
,RRf g
TestOptionsRRh s
?RRs t
testOptions	RRu Ä
=
RRÅ Ç
null
RRÉ á
)
RRá à
{SS 
ifTT 

(TT 
_mockConfigurationTT 
isTT !
notTT" %
nullTT& *
)TT* +
{UU 	
EnvironmentVV 
.VV "
SetEnvironmentVariableVV .
(VV. /(
PulumiConfigurationConstantsVV/ K
.VVK L
EnvironmentVariableVVL _
,VV_ `
JsonSerializerWW 
.WW 
	SerializeWW (
(WW( )
_mockConfigurationWW) ;
.WW; <
MockConfigurationsWW< N
)WWN O
)WWO P
;WWP Q
}XX 	
varZZ 
mocksZZ 
=ZZ 
newZZ 
MocksZZ 
.ZZ 
MocksZZ #
(ZZ# $
_mockResourcesZZ$ 2
.ZZ2 3!
ToImmutableDictionaryZZ3 H
(ZZH I
)ZZI J
,ZZJ K

_mockCallsZZL V
.ZZV W!
ToImmutableDictionaryZZW l
(ZZl m
)ZZm n
)ZZn o
;ZZo p
(\\ 	
ImmutableArray\\	 
<\\ 
Resource\\  
>\\  !
stackResources\\" 0
,\\0 1
IDictionary\\2 =
<\\= >
string\\> D
,\\D E
object\\F L
?\\L M
>\\M N
stackOutputs\\O [
)\\[ \
=\\] ^
await\\_ d

Deployment\\e o
.\\o p
	TestAsync\\p y
(\\y z
mocks]] 
,]] 
testOptions^^ 
??^^ 
new^^ 
TestOptions^^ *
{^^+ ,
	IsPreview^^- 6
=^^7 8
false^^9 >
}^^? @
,^^@ A
async__ 
(__ 
)__ 
=>__ 
await__ 
createResourcesFunc__ 1
(__1 2
)__2 3
)__3 4
;__4 5
returnaa 
newaa 
Fixtureaa 
(aa 
stackResourcesaa )
,aa) *
stackOutputsaa+ 7
.aa7 8!
ToImmutableDictionaryaa8 M
(aaM N
)aaN O
,aaO P
mocksaaQ V
.aaV W
ResourceSnapshotsaaW h
,aah i
mocksaaj o
.aao p
CallSnapshotsaap }
)aa} ~
;aa~ 
}bb 
}cc ¿
W/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Pulumock/TestFixtures/Fixture.cs
	namespace 	
Pulumock
 
. 
TestFixtures 
;  
public 
record 
Fixture 
( 
ImmutableArray $
<$ %
Resource% -
>- .
StackResources/ =
,= >
ImmutableDictionary 
< 
string 
, 
object  &
?& '
>' (
StackOutputs) 5
,5 6
ImmutableList		 
<		 
ResourceSnapshot		 "
>		" #
ResourceSnapshots		$ 5
,		5 6
ImmutableList

 
<

 
CallSnapshot

 
>

 
CallSnapshots

  -
)

- .
;

. /ä
v/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Pulumock/TestFixtures/Constants/PulumiConfigurationConstants.cs
	namespace 	
Pulumock
 
. 
TestFixtures 
.  
	Constants  )
;) *
public 
static 
class (
PulumiConfigurationConstants 0
{ 
public 

const 
string 
EnvironmentVariable +
=, -
$str. =
;= >
} ¨
`/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Pulumock/Mocks/Models/ResourceSnapshot.cs
	namespace 	
Pulumock
 
. 
Mocks 
. 
Models 
;  
public 
sealed 
record 
ResourceSnapshot %
(% &
string& ,
LogicalName- 8
,8 9
ImmutableDictionary: M
<M N
stringN T
,T U
objectV \
>\ ]
Inputs^ d
)d e
;e fÍ
b/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Pulumock/Mocks/Models/MockStackReference.cs
	namespace 	
Pulumock
 
. 
Mocks 
. 
Models 
;  
public 
record 
MockStackReference  
(  !
string! '#
FullyQualifiedStackName( ?
,? @
ImmutableDictionaryA T
<T U
stringU [
,[ \
object] c
>c d
MockOutputse p
)p q
: 
MockResource 
( 
typeof 
( 
StackReference (
)( )
,) *
MockOutputs+ 6
)6 7
;7 8¯
\/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Pulumock/Mocks/Models/MockResource.cs
	namespace 	
Pulumock
 
. 
Mocks 
. 
Models 
;  
public 
record 
MockResource 
( 
Type 
Type  $
,$ %
ImmutableDictionary& 9
<9 :
string: @
,@ A
objectB H
>H I
MockOutputsJ U
,U V
stringW ]
?] ^
LogicalName_ j
=k l
nullm q
)q r
;r sÍ
a/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Pulumock/Mocks/Models/MockConfiguration.cs
	namespace 	
Pulumock
 
. 
Mocks 
. 
Models 
;  
public 
record 
MockConfiguration 
(  
ImmutableDictionary  3
<3 4
string4 :
,: ;
object< B
>B C
MockConfigurationsD V
)V W
;W Xœ&
]/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Pulumock/Mocks/Models/MockCallToken.cs
	namespace 	
Pulumock
 
. 
Mocks 
. 
Models 
;  
public 
record 
MockCallToken 
{ 
private 
Type 
? 
	TypeToken 
{ 
get !
;! "
}# $
private 
string 
? 
StringToken 
{  !
get" %
;% &
}' (
private

 
MockCallToken

 
(

 
Type

 
?

 
	typeToken

  )
,

) *
string

+ 1
?

1 2
stringToken

3 >
)

> ?
{ 
	TypeToken 
= 
	typeToken 
; 
StringToken 
= 
stringToken !
;! "
} 
public 

static 
MockCallToken 
FromStringToken  /
(/ 0
string0 6
stringToken7 B
)B C
{ 
if 

( 
string 
. 
IsNullOrWhiteSpace %
(% &
stringToken& 1
)1 2
)2 3
{ 	
throw 
new 
ArgumentException '
(' (
$str( K
,K L
nameofM S
(S T
stringTokenT _
)_ `
)` a
;a b
} 	
return 
new 
MockCallToken  
(  !
null! %
,% &
stringToken' 2
)2 3
;3 4
} 
public 

static 
MockCallToken 
FromTypeToken  -
(- .
Type. 2
	typeToken3 <
)< =
{ !
ArgumentNullException 
. 
ThrowIfNull )
() *
	typeToken* 3
)3 4
;4 5
return 
new 
MockCallToken  
(  !
	typeToken! *
,* +
null, 0
)0 1
;1 2
} 
public   

bool   
IsStringToken   
=>    
StringToken  ! ,
is  - /
not  0 3
null  4 8
;  8 9
public!! 

bool!! 
IsTypeToken!! 
=>!! 
	TypeToken!! (
is!!) +
not!!, /
null!!0 4
;!!4 5
public## 

string## 
StringTokenValue## "
=>### %
StringToken$$ 
??$$ 
throw$$ 
new$$  %
InvalidOperationException$$! :
($$: ;
$str$$; a
)$$a b
;$$b c
public&& 

Type&& 
TypeTokenValue&& 
=>&& !
	TypeToken'' 
??'' 
throw'' 
new'' %
InvalidOperationException'' 8
(''8 9
$str''9 ^
)''^ _
;''_ `
public)) 

bool)) 
ConflictsWith)) 
()) 
MockCallToken)) +
other)), 1
)))1 2
{** 
if++ 

(++ 
IsStringToken++ 
&&++ 
other++ "
.++" #
IsStringToken++# 0
)++0 1
{,, 	
return-- 
string-- 
.-- 
Equals--  
(--  !
StringTokenValue--! 1
,--1 2
other--3 8
.--8 9
StringTokenValue--9 I
,--I J
StringComparison--K [
.--[ \
Ordinal--\ c
)--c d
;--d e
}.. 	
if00 

(00 
IsTypeToken00 
&&00 
other00  
.00  !
IsTypeToken00! ,
)00, -
{11 	
return22 
TypeTokenValue22 !
==22" $
other22% *
.22* +
TypeTokenValue22+ 9
;229 :
}33 	
if55 

(55 
IsTypeToken55 
&&55 
other55  
.55  !
IsStringToken55! .
)55. /
{66 	
return77 
TypeTokenValue77 !
.77! " 
MatchesCallTypeToken77" 6
(776 7
other777 <
.77< =
StringTokenValue77= M
)77M N
;77N O
}88 	
if:: 

(:: 
IsStringToken:: 
&&:: 
other:: "
.::" #
IsTypeToken::# .
)::. /
{;; 	
return<< 
other<< 
.<< 
TypeTokenValue<< '
.<<' ( 
MatchesCallTypeToken<<( <
(<<< =
StringTokenValue<<= M
)<<M N
;<<N O
}== 	
return?? 
false?? 
;?? 
}@@ 
}AA é
X/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Pulumock/Mocks/Models/MockCall.cs
	namespace 	
Pulumock
 
. 
Mocks 
. 
Models 
;  
public 
record 
MockCall 
( 
MockCallToken $
Token% *
,* +
ImmutableDictionary, ?
<? @
string@ F
,F G
objectH N
>N O
MockOutputsP [
)[ \
;\ ]æ
\/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Pulumock/Mocks/Models/CallSnapshot.cs
	namespace 	
Pulumock
 
. 
Mocks 
. 
Models 
;  
public 
sealed 
record 
CallSnapshot !
(! "
string" (
Token) .
,. /
ImmutableDictionary0 C
<C D
stringD J
,J K
objectL R
>R S
InputsT Z
,Z [
ImmutableDictionary\ o
<o p
stringp v
,v w
objectx ~
>~ 
Outputs
Ä á
)
á à
;
à â±B
N/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Pulumock/Mocks/Mocks.cs
	namespace 	
Pulumock
 
. 
Mocks 
; 
internal 
sealed	 
class 
Mocks 
( 
ImmutableDictionary /
</ 0
(0 1
Type1 5
Type6 :
,: ;
string< B
?B C
LogicalNameD O
)O P
,P Q
MockResourceR ^
>^ _
mockResources` m
,m n
ImmutableDictionary 
< 
MockCallToken %
,% &
MockCall' /
>/ 0
	mockCalls1 :
): ;
:< =
IMocks> D
{ 
private 
readonly 
List 
< 
ResourceSnapshot *
>* +
_resourceSnapshots, >
=? @
[A B
]B C
;C D
private 
readonly 
List 
< 
CallSnapshot &
>& '
_callSnapshots( 6
=7 8
[9 :
]: ;
;; <
public 

Task 
< 
( 
string 
? 
id 
, 
object #
state$ )
)) *
>* +
NewResourceAsync, <
(< =
MockResourceArgs= M
argsN R
)R S
{ 
ImmutableDictionary 
< 
string "
," #
object$ *
>* +
.+ ,
Builder, 3
outputs4 ;
=< =
ImmutableDictionary> Q
.Q R
CreateBuilderR _
<_ `
string` f
,f g
objecth n
>n o
(o p
)p q
;q r
if 

( 

MockHelper 
. 
IsStackReference '
(' (
args( ,
), -
)- .
{ 	
if 
( 
mockResources 
. 
TryGetValue )
() *
(* +
typeof+ 1
(1 2
StackReference2 @
)@ A
,A B

MockHelperC M
.M N"
GetLogicalResourceNameN d
(d e
argse i
.i j
Namej n
)n o
)o p
,p q
outr u
MockResource	v Ç
?
Ç É
mockResource
Ñ ê
)
ê ë
)
ë í
{ 
outputs 
. 
Add 
( 
$str %
,% &
mockResource' 3
.3 4
MockOutputs4 ?
)? @
;@ A
outputs 
. 
Add 
( 
$str /
,/ 0
ImmutableArray1 ?
<? @
string@ F
>F G
.G H
EmptyH M
)M N
;N O
}   
}!! 	
else"" 
{## 	
MockResource$$ 
?$$ 
mockResource$$ &
=$$' (

MockHelper$$) 3
.$$3 4$
GetMockResourceOrDefault$$4 L
($$L M
mockResources$$M Z
,$$Z [
args$$\ `
.$$` a
Type$$a e
,$$e f
args$$g k
.$$k l
Name$$l p
)$$p q
;$$q r
if%% 
(%% 
mockResource%% 
is%% 
not%%  #
null%%$ (
)%%( )
{&& 
outputs'' 
.'' 
AddRange''  
(''  !
mockResource''! -
.''- .
MockOutputs''. 9
)''9 :
;'': ;
}(( 
outputs** 
.** 
Add** 
(** 
$str** 
,** 

MockHelper**  *
.*** +#
GetPhysicalResourceName**+ B
(**B C
args**C G
,**G H
outputs**I P
)**P Q
)**Q R
;**R S
}++ 	
string-- 
resourceName-- 
=-- 

MockHelper-- (
.--( )"
GetLogicalResourceName--) ?
(--? @
args--@ D
.--D E
Name--E I
)--I J
;--J K
string.. 

resourceId.. 
=.. 

MockHelper.. &
...& '
GetResourceId..' 4
(..4 5
args..5 9
...9 :
Id..: <
,..< =
$"..> @
{..@ A
resourceName..A M
}..M N
$str..N Q
"..Q R
)..R S
;..S T
ImmutableDictionary00 
<00 
string00 "
,00" #
object00$ *
>00* +
mergedOutputs00, 9
=00: ;
OutputMerger00< H
.00H I
Merge00I N
(00N O
args00O S
.00S T
Inputs00T Z
,00Z [
outputs00\ c
)00c d
;00d e
_resourceSnapshots22 
.22 
Add22 
(22 
new22 "
ResourceSnapshot22# 3
(223 4
resourceName224 @
,22@ A
args22B F
.22F G
Inputs22G M
)22M N
)22N O
;22O P
return33 
Task33 
.33 

FromResult33 
<33 
(33  
string33  &
?33& '
,33' (
object33) /
)33/ 0
>330 1
(331 2
(332 3

resourceId333 =
,33= >
mergedOutputs33? L
)33L M
)33M N
;33N O
}44 
public66 

Task66 
<66 
object66 
>66 
	CallAsync66 !
(66! "
MockCallArgs66" .
args66/ 3
)663 4
{77 
string88 
	callToken88 
=88 

MockHelper88 %
.88% &
GetCallToken88& 2
(882 3
args883 7
.887 8
Token888 =
)88= >
;88> ?
ImmutableDictionary:: 
<:: 
string:: "
,::" #
object::$ *
>::* +
.::+ ,
Builder::, 3
outputs::4 ;
=::< =
ImmutableDictionary::> Q
.::Q R
CreateBuilder::R _
<::_ `
string::` f
,::f g
object::h n
>::n o
(::o p
)::p q
;::q r
MockCall<< 
?<< 
mockCall<< 
=<< 

MockHelper<< '
.<<' ( 
GetMockCallOrDefault<<( <
(<<< =
	mockCalls<<= F
,<<F G
	callToken<<H Q
)<<Q R
;<<R S
if== 

(== 
mockCall== 
is== 
not== 
null==  
)==  !
{>> 	
outputs?? 
.?? 
AddRange?? 
(?? 
mockCall?? %
.??% &
MockOutputs??& 1
)??1 2
;??2 3
}@@ 	
ImmutableDictionaryBB 
<BB 
stringBB "
,BB" #
objectBB$ *
>BB* +
mergedOutputsBB, 9
=BB: ;
OutputMergerBB< H
.BBH I
MergeBBI N
(BBN O
argsBBO S
.BBS T
ArgsBBT X
,BBX Y
outputsBBZ a
)BBa b
;BBb c
_callSnapshotsDD 
.DD 
AddDD 
(DD 
newDD 
CallSnapshotDD +
(DD+ ,
	callTokenDD, 5
,DD5 6
argsDD7 ;
.DD; <
ArgsDD< @
,DD@ A
mergedOutputsDDB O
)DDO P
)DDP Q
;DDQ R
returnFF 
TaskFF 
.FF 

FromResultFF 
<FF 
objectFF %
>FF% &
(FF& '
mergedOutputsFF' 4
)FF4 5
;FF5 6
}GG 
publicII 

ImmutableListII 
<II 
ResourceSnapshotII )
>II) *
ResourceSnapshotsII+ <
=>II= ?
_resourceSnapshotsII@ R
.IIR S
ToImmutableListIIS b
(IIb c
)IIc d
;IId e
publicJJ 

ImmutableListJJ 
<JJ 
CallSnapshotJJ %
>JJ% &
CallSnapshotsJJ' 4
=>JJ5 7
_callSnapshotsJJ8 F
.JJF G
ToImmutableListJJG V
(JJV W
)JJW X
;JJX Y
}KK ı
m/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Pulumock/Mocks/Constants/ResourceTypeTokenConstants.cs
	namespace 	
Pulumock
 
. 
Mocks 
. 
	Constants "
;" #
internal		 
static			 
class		 &
ResourceTypeTokenConstants		 0
{

 
public 

const 
string 
StackReference &
=' (
$str) G
;G H
} ⁄
k/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Pulumock/Mocks/Enums/PulumiConfigurationNamespace.cs
	namespace 	
Pulumock
 
. 
Mocks 
. 
Enums 
; 
public 
sealed 
class (
PulumiConfigurationNamespace 0
{ 
public 

static 
readonly (
PulumiConfigurationNamespace 7
Default8 ?
=@ A
newB E
(E F
$strF O
)O P
;P Q
public 

static 
readonly (
PulumiConfigurationNamespace 7
Aws8 ;
=< =
new> A
(A B
$strB G
)G H
;H I
public 

static 
readonly (
PulumiConfigurationNamespace 7
AzureClassic8 D
=E F
newG J
(J K
$strK R
)R S
;S T
public 

static 
readonly (
PulumiConfigurationNamespace 7
AzureNative8 C
=D E
newF I
(I J
$strJ X
)X Y
;Y Z
public 

static 
readonly (
PulumiConfigurationNamespace 7
GoogleCloudClassic8 J
=K L
newM P
(P Q
$strQ V
)V W
;W X
public 

static 
readonly (
PulumiConfigurationNamespace 7
GoogleCloudNative8 I
=J K
newL O
(O P
$strP _
)_ `
;` a
public 

static 
readonly (
PulumiConfigurationNamespace 7

Kubernetes8 B
=C D
newE H
(H I
$strI U
)U V
;V W
public 

string 
Value 
{ 
get 
; 
}  
private (
PulumiConfigurationNamespace (
(( )
string) /
value0 5
)5 6
=> 

Value 
= 
value 
; 
} Ô
f/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Pulumock/Mocks/Builders/NestedOutputsBuilder.cs
	namespace 	
Pulumock
 
. 
Mocks 
. 
Builders !
;! "
public 
class  
NestedOutputsBuilder !
<! "
T" #
># $
{ 
private 
readonly 

Dictionary 
<  
string  &
,& '
object( .
>. /
_values0 7
=8 9
new: =
(= >
)> ?
;? @
public

 
 
NestedOutputsBuilder

 
<

  
T

  !
>

! "
WithNestedOutput

# 3
(

3 4

Expression

4 >
<

> ?
Func

? C
<

C D
T

D E
,

E F
object

G M
>

M N
>

N O
selector

P X
,

X Y
object

Z `
value

a f
)

f g
{ 
_values 
[ 
selector 
. 
GetOutputName &
(& '
)' (
]( )
=* +
value, 1
;1 2
return 
this 
; 
} 
public 


Dictionary 
< 
string 
, 
object $
>$ %
Build& +
(+ ,
), -
=>. 0
_values1 8
;8 9
} ®
k/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Pulumock/Mocks/Builders/MockStackReferenceBuilder.cs
	namespace 	
Pulumock
 
. 
Mocks 
. 
Builders !
;! "
public 
class %
MockStackReferenceBuilder &
(& '
string' -#
fullyQualifiedStackName. E
)E F
{ 
private 
readonly 

Dictionary 
<  
string  &
,& '
object( .
>. /
_outputs0 8
=9 :
new; >
(> ?
)? @
;@ A
public 
%
MockStackReferenceBuilder $

WithOutput% /
(/ 0
string0 6
key7 :
,: ;
object< B
valueC H
)H I
{ 
_outputs 
. 
Add 
( 
key 
, 
value 
)  
;  !
return 
this 
; 
} 
public 

MockStackReference 
Build #
(# $
)$ %
=>& (
new 
( #
fullyQualifiedStackName #
,# $
_outputs% -
.- .!
ToImmutableDictionary. C
(C D
)D E
)E F
;F G
}   ≈
e/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Pulumock/Mocks/Builders/MockResourceBuilder.cs
	namespace 	
Pulumock
 
. 
Mocks 
. 
Builders !
;! "
public 
class 
MockResourceBuilder  
{ 
private 
readonly 

Dictionary 
<  
string  &
,& '
object( .
>. /
_outputs0 8
=9 :
new; >
(> ?
)? @
;@ A
public 

MockResourceBuilder 

WithOutput )
() *
string* 0
key1 4
,4 5
object6 <
value= B
)B C
{ 
_outputs 
. 
Add 
( 
key 
, 
value 
)  
;  !
return 
this 
; 
} 
public## 

MockResourceBuilder## 

WithOutput## )
<##) *
T##* +
>##+ ,
(##, -

Expression##- 7
<##7 8
Func##8 <
<##< =
T##= >
,##> ?
object##@ F
>##F G
>##G H
propertySelector##I Y
,##Y Z
object##[ a
value##b g
)##g h
{$$ 
_outputs%% 
.%% 
Add%% 
(%% 
propertySelector%% %
.%%% &
GetOutputName%%& 3
(%%3 4
)%%4 5
,%%5 6
value%%7 <
)%%< =
;%%= >
return&& 
this&& 
;&& 
}'' 
public)) 

MockResourceBuilder)) 

WithOutput)) )
<))) *
T))* +
,))+ ,
TNested))- 4
>))4 5
())5 6

Expression** 
<** 
Func** 
<** 
T** 
,** 
object** !
>**! "
>**" #
propertySelector**$ 4
,**4 5
Func++ 
<++  
NestedOutputsBuilder++ !
<++! "
TNested++" )
>++) *
,++* + 
NestedOutputsBuilder++, @
<++@ A
TNested++A H
>++H I
>++I J 
nestedOutputsBuilder++K _
)++_ `
{,, 

Dictionary-- 
<-- 
string-- 
,-- 
object-- !
>--! "
nestedOutputs--# 0
=--1 2 
nestedOutputsBuilder--3 G
(--G H
new--H K 
NestedOutputsBuilder--L `
<--` a
TNested--a h
>--h i
(--i j
)--j k
)--k l
... 
Build.. 
(.. 
).. 
;.. 
_outputs00 
.00 
Add00 
(00 
propertySelector00 %
.00% &
GetOutputName00& 3
(003 4
)004 5
,005 6
nestedOutputs007 D
)00D E
;00E F
return11 
this11 
;11 
}22 
public77 

MockResource77 
Build77 
<77 
T77 
>77  
(77  !
string77! '
?77' (
logicalName77) 4
=775 6
null777 ;
)77; <
=>77= ?
new88 
(88 
typeof88 
(88 
T88 
)88 
,88 
_outputs88 
.88  !
ToImmutableDictionary88  5
(885 6
)886 7
,887 8
logicalName889 D
)88D E
;88E F
}99 ÿ
j/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Pulumock/Mocks/Builders/MockConfigurationBuilder.cs
	namespace 	
Pulumock
 
. 
Mocks 
. 
Builders !
;! "
public

 
class

 $
MockConfigurationBuilder

 %
{ 
private 
readonly 

Dictionary 
<  
string  &
,& '
object( .
>. /
_configurations0 ?
=@ A
newB E
(E F
)F G
;G H
public 
$
MockConfigurationBuilder #
WithConfiguration$ 5
(5 6
string6 <
key= @
,@ A
objectB H
valueI N
)N O
{ 
_configurations 
. 
Add 
( 
key 
,  
value! &
)& '
;' (
return 
this 
; 
} 
public 
$
MockConfigurationBuilder ##
WithSecretConfiguration$ ;
(; <
string< B
keyC F
,F G
stringH N
secretO U
)U V
{   
WithConfiguration!! 
(!! 
key!! 
,!! 
secret!! %
)!!% &
;!!& '
return## 
this## 
;## 
}$$ 
public-- 
$
MockConfigurationBuilder-- #
WithConfiguration--$ 5
(--5 6(
PulumiConfigurationNamespace--6 R

@namespace--S ]
,--] ^
string--_ e
keyName--f m
,--m n
object--o u
value--v {
)--{ |
{.. 
string// 
key// 
=// 
	FormatKey// 
(// 

@namespace// )
.//) *
Value//* /
,/// 0
keyName//1 8
)//8 9
;//9 :
WithConfiguration11 
(11 
key11 
,11 
value11 $
)11$ %
;11% &
return33 
this33 
;33 
}44 
public== 
$
MockConfigurationBuilder== ##
WithSecretConfiguration==$ ;
(==; <(
PulumiConfigurationNamespace==< X

@namespace==Y c
,==c d
string==e k
keyName==l s
,==s t
string==u {
value	==| Å
)
==Å Ç
{>> 
string?? 
key?? 
=?? 
	FormatKey?? 
(?? 

@namespace?? )
.??) *
Value??* /
,??/ 0
keyName??1 8
)??8 9
;??9 :#
WithSecretConfigurationAA 
(AA  
keyAA  #
,AA# $
valueAA% *
)AA* +
;AA+ ,
returnCC 
thisCC 
;CC 
}DD 
publicII 

MockConfigurationII 
BuildII "
(II" #
)II# $
=>II% '
newJJ 
(JJ 
_configurationsJJ 
.JJ !
ToImmutableDictionaryJJ 1
(JJ1 2
)JJ2 3
)JJ3 4
;JJ4 5
privateLL 
staticLL 
stringLL 
	FormatKeyLL #
(LL# $
stringLL$ *

@namespaceLL+ 5
,LL5 6
stringLL7 =
?LL= >
keyNameLL? F
)LLF G
=>LLH J
stringMM 
.MM 
IsNullOrWhiteSpaceMM !
(MM! "
keyNameMM" )
)MM) *
?MM+ ,

@namespaceMM- 7
:MM8 9
$"MM: <
{MM< =

@namespaceMM= G
}MMG H
$strMMH I
{MMI J
keyNameMMJ Q
}MMQ R
"MMR S
;MMS T
}NN ê
a/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Pulumock/Mocks/Builders/MockCallBuilder.cs
	namespace 	
Pulumock
 
. 
Mocks 
. 
Builders !
;! "
public 
class 
MockCallBuilder 
{ 
private 
readonly 

Dictionary 
<  
string  &
,& '
object( .
>. /
_outputs0 8
=9 :
new; >
(> ?
)? @
;@ A
public 

MockCallBuilder 

WithOutput %
(% &
string& ,
key- 0
,0 1
object2 8
value9 >
)> ?
{ 
_outputs 
. 
Add 
( 
key 
, 
value 
)  
;  !
return 
this 
; 
} 
public   

MockCallBuilder   

WithOutput   %
<  % &
T  & '
>  ' (
(  ( )

Expression  ) 3
<  3 4
Func  4 8
<  8 9
T  9 :
,  : ;
object  < B
>  B C
>  C D
propertySelector  E U
,  U V
object  W ]
value  ^ c
)  c d
{!! 
_outputs"" 
."" 
Add"" 
("" 
propertySelector"" %
.""% &
GetOutputName""& 3
(""3 4
)""4 5
,""5 6
value""7 <
)""< =
;""= >
return## 
this## 
;## 
}$$ 
public&& 

MockCallBuilder&& 

WithOutput&& %
<&&% &
T&&& '
,&&' (
TNested&&) 0
>&&0 1
(&&1 2

Expression'' 
<'' 
Func'' 
<'' 
T'' 
,'' 
object'' !
>''! "
>''" #
propertySelector''$ 4
,''4 5
Func(( 
<((  
NestedOutputsBuilder(( !
<((! "
TNested((" )
>(() *
,((* + 
NestedOutputsBuilder((, @
<((@ A
TNested((A H
>((H I
>((I J 
nestedOutputsBuilder((K _
)((_ `
{)) 

Dictionary** 
<** 
string** 
,** 
object** !
>**! "
nestedOutputs**# 0
=**1 2 
nestedOutputsBuilder**3 G
(**G H
new**H K 
NestedOutputsBuilder**L `
<**` a
TNested**a h
>**h i
(**i j
)**j k
)**k l
.++ 
Build++ 
(++ 
)++ 
;++ 
_outputs-- 
.-- 
Add-- 
(-- 
propertySelector-- %
.--% &
GetOutputName--& 3
(--3 4
)--4 5
,--5 6
nestedOutputs--7 D
)--D E
;--E F
return.. 
this.. 
;.. 
}// 
public55 

MockCall55 
Build55 
(55 
Type55 
type55 #
)55# $
=>55% '
new66 
(66 
MockCallToken66 
.66 
FromTypeToken66 '
(66' (
type66( ,
)66, -
,66- .
_outputs66/ 7
.667 8!
ToImmutableDictionary668 M
(66M N
)66N O
)66O P
;66P Q
public<< 

MockCall<< 
Build<< 
(<< 
string<<  
token<<! &
)<<& '
=><<( *
new== 
(== 
MockCallToken== 
.== 
FromStringToken== )
(==) *
token==* /
)==/ 0
,==0 1
_outputs==2 :
.==: ;!
ToImmutableDictionary==; P
(==P Q
)==Q R
)==R S
;==S T
}>> ª+
\/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Pulumock/Extensions/TypeExtensions.cs
	namespace 	
Pulumock
 
. 

Extensions 
; 
public

 
static

 
class

 
TypeExtensions

 "
{ 
public 

static 
bool $
MatchesResourceTypeToken /
(/ 0
this0 4
Type5 9
type: >
,> ?
string@ F
?F G
tokenH M
)M N
=>O Q
! 	
string	 
. 
IsNullOrWhiteSpace "
(" #
token# (
)( )
&&* ,
type 
. !
GetResourceTypeTokens "
(" #
)# $
.$ %
Contains% -
(- .
token. 3
)3 4
;4 5
public 

static 
bool  
MatchesCallTypeToken +
(+ ,
this, 0
Type1 5
type6 :
,: ;
string< B
?B C
tokenD I
)I J
=>K M
! 	
string	 
. 
IsNullOrWhiteSpace "
(" #
token# (
)( )
&&* ,
token 
. 
Contains 
( 
type 
. 
Name  
,  !
StringComparison" 2
.2 3
OrdinalIgnoreCase3 D
)D E
;E F
private&& 
static&& 
IEnumerable&& 
<&& 
string&& %
>&&% &!
GetResourceTypeTokens&&' <
(&&< =
this&&= A
Type&&B F
type&&G K
)&&K L
=>&&M O
type'' 
.(( 
GetCustomAttributes((  
(((  !
inherit((! (
:((( )
false((* /
)((/ 0
.)) 
Select)) 
()) 
attr)) 
=>)) 
attr))  
.** 
GetType** 
(** 
)** 
.++ 
GetProperty++ 
(++ '
PulumiTypeTokenPropertyName++ 8
)++8 9
?++9 :
.,, 
GetValue,, 
(,, 
attr,, 
),, 
),,  
.-- 
OfType-- 
<-- 
string-- 
>-- 
(-- 
)-- 
... 
Where.. 
(.. 
token.. 
=>.. 
!.. 
string.. #
...# $
IsNullOrWhiteSpace..$ 6
(..6 7
token..7 <
)..< =
)..= >
;..> ?
public>> 

static>> 
string>> 
GetOutputName>> &
<>>& '
T>>' (
>>>( )
(>>) *
this>>* .

Expression>>/ 9
<>>9 :
Func>>: >
<>>> ?
T>>? @
,>>@ A
object>>B H
>>>H I
>>>I J
propertySelector>>K [
)>>[ \
{?? 

MemberInfo@@ 
member@@ 
=@@ 
propertySelector@@ ,
.@@, -
Body@@- 1
switch@@2 8
{AA 	
MemberExpressionBB 
mBB 
=>BB !
mBB" #
.BB# $
MemberBB$ *
,BB* +
UnaryExpressionCC 
{CC 
OperandCC %
:CC% &
MemberExpressionCC' 7
mCC8 9
}CC: ;
=>CC< >
mCC? @
.CC@ A
MemberCCA G
,CCG H
_DD 
=>DD 
throwDD 
newDD 
ArgumentExceptionDD ,
(DD, -
$strDD- H
)DDH I
}EE 	
;EE	 

returnGG 
memberGG 
.GG 
GetCustomAttributeGG (
<GG( )
OutputAttributeGG) 8
>GG8 9
(GG9 :
)GG: ;
?GG; <
.GG< =
NameGG= A
??GGB D
memberGGE K
.GGK L
NameGGL P
.GGP Q
ToCamelCaseGGQ \
(GG\ ]
)GG] ^
;GG^ _
}HH 
publicXX 

staticXX 
stringXX 
GetInputNameXX %
<XX% &
TXX& '
>XX' (
(XX( )
thisXX) -

ExpressionXX. 8
<XX8 9
FuncXX9 =
<XX= >
TXX> ?
,XX? @
objectXXA G
?XXG H
>XXH I
>XXI J
propertySelectorXXK [
)XX[ \
{YY 

MemberInfoZZ 
memberZZ 
=ZZ 
propertySelectorZZ ,
.ZZ, -
BodyZZ- 1
switchZZ2 8
{[[ 	
MemberExpression\\ 
m\\ 
=>\\ !
m\\" #
.\\# $
Member\\$ *
,\\* +
UnaryExpression]] 
{]] 
Operand]] %
:]]% &
MemberExpression]]' 7
m]]8 9
}]]: ;
=>]]< >
m]]? @
.]]@ A
Member]]A G
,]]G H
_^^ 
=>^^ 
throw^^ 
new^^ 
ArgumentException^^ ,
(^^, -
$str^^- H
)^^H I
}__ 	
;__	 

returnbb 
memberbb 
.bb 
Namebb 
.bb 
ToCamelCasebb &
(bb& '
)bb' (
;bb( )
}cc 
privatehh 
consthh 
stringhh '
PulumiTypeTokenPropertyNamehh 4
=hh5 6
$strhh7 =
;hh= >
}ii ∏	
^/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Pulumock/Extensions/StringExtensions.cs
	namespace 	
Pulumock
 
. 

Extensions 
; 
public 
static 
class 
StringExtensions $
{ 
public 

static 
string 
ToCamelCase $
($ %
this% )
string* 0
name1 5
)5 6
{ 
if 

( 
string 
. 
IsNullOrWhiteSpace %
(% &
name& *
)* +
||, .
char/ 3
.3 4
IsLower4 ;
(; <
name< @
[@ A
$numA B
]B C
)C D
)D E
{ 	
return 
name 
; 
} 	
return 
char 
. 
ToLowerInvariant $
($ %
name% )
[) *
$num* +
]+ ,
), -
+. /
name0 4
[4 5
$num5 6
..6 8
]8 9
;9 :
} 
} ‰	
`/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Pulumock/Extensions/ResourceExtensions.cs
	namespace 	
Pulumock
 
. 

Extensions 
; 
public		 
static		 
class		 
ResourceExtensions		 &
{

 
public 

static 
T $
GetResourceByLogicalName ,
<, -
T- .
>. /
(/ 0
this0 4
ImmutableArray5 C
<C D
ResourceD L
>L M
	resourcesN W
,W X
stringY _
logicalName` k
)k l
wherem r
Ts t
:u v
Resourcew 
=>
Ä Ç
	resources 
. 
OfType 
< 
T 
> 
( 
) 
. 
Single 
( 
x 
=> 
x 
. 
GetResourceName *
(* +
)+ ,
., -
Equals- 3
(3 4
logicalName4 ?
,? @
StringComparisonA Q
.Q R
OrdinalR Y
)Y Z
)Z [
;[ \
} §
^/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Pulumock/Extensions/OutputExtensions.cs
	namespace 	
Pulumock
 
. 

Extensions 
; 
public		 
static		 
class		 
OutputExtensions		 $
{

 
public 

static 
async 
Task 
< 
T 
> 
GetValueAsync  -
<- .
T. /
>/ 0
(0 1
this1 5
Output6 <
<< =
T= >
>> ?
output@ F
)F G
=>H J
await 
OutputUtilities 
. 
GetValueAsync +
(+ ,
output, 2
)2 3
;3 4
} _
]/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Pulumock/Extensions/InputExtensions.cs