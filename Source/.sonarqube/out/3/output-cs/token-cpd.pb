Í
j/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Example.Tests.WithoutPulumock/Shared/TestHelpers.cs
	namespace 	
Example
 
. 
Tests 
. 
WithoutPulumock '
.' (
Shared( .
;. /
internal 
static	 
class 
TestHelpers !
{ 
public 

static 
bool 
	IsChildOf  
(  !
Resource! )
child* /
,/ 0
Resource1 9
potentialParent: I
)I J
{		 
PropertyInfo

 
?

 
childResourcesField

 )
=

* +
typeof

, 2
(

2 3
Resource

3 ;
)

; <
. 
GetProperty 
( 
$str )
,) *
BindingFlags+ 7
.7 8
Instance8 @
|A B
BindingFlagsC O
.O P
	NonPublicP Y
|Z [
BindingFlags\ h
.h i
Publici o
)o p
;p q
if 

( 
childResourcesField 
is  "
null# '
)' (
{ 	
throw 
new %
InvalidOperationException /
(/ 0
$str0 Y
)Y Z
;Z [
} 	
var 
children 
= 
childResourcesField *
.* +
GetValue+ 3
(3 4
potentialParent4 C
)C D
asE G
IEnumerableH S
<S T
ResourceT \
>\ ]
;] ^
return 
children 
? 
. 
Contains !
(! "
child" '
)' (
??) +
false, 1
;1 2
} 
} è
g/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Example.Tests.WithoutPulumock/Shared/TestBase.cs
	namespace 	
Example
 
. 
Tests 
. 
WithoutPulumock '
.' (
Shared( .
;. /
public 
class 
TestBase 
{ 
	protected		 
const		 
string		 
	StackName		 $
=		% &
$str		' ,
;		, -
	protected 
TestBase 
( 
) 
=> 
Environment 
. "
SetEnvironmentVariable *
(* +
$str+ :
,: ;
JsonSerializer< J
.J K
	SerializeK T
(T U
newU X

DictionaryY c
<c d
stringd j
,j k
objectl r
>r s
{ 	
{ 
$str %
,% &
$str' M
}N O
,O P
{ 
$str +
,+ ,
$str- S
}T U
,U V
{ 
$str %
,% &
$str' 6
}7 8
,8 9
{ 
$str -
,- .
$str/ 7
}8 9
,9 :
{ 
$str 1
,1 2
$str3 C
}D E
,E F
{ 
$str 0
,0 1
$str2 E
}F G
} 	
)	 

)
 
; 
} ü
k/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Example.Tests.WithoutPulumock/StackReferenceTests.cs
	namespace 	
Example
 
. 
Tests 
. 
WithoutPulumock '
;' (
public 
class 
StackReferenceTests  
:! "
TestBase# +
,+ , 
IStackReferenceTests- A
{ 
[ 
Fact 	
]	 

public 

async 
Task B
6StackReference_ShouldUseMockedStackReferenceInResource L
(L M
)M N
{ 
( 	
ImmutableArray	 
< 
Resource  
>  !
	Resources" +
,+ ,
IDictionary- 8
<8 9
string9 ?
,? @
objectA G
?G H
>H I
StackOutputsJ V
)V W
resultX ^
=_ `
awaita f

Deploymentg q
.q r
	TestAsyncr {
({ |
new 
Mocks 
. 
Mocks 
( 
) 
, 
new 
TestOptions 
{ 
	IsPreview &
=' (
false) .
}. /
,/ 0
async 
( 
) 
=> 
await 
	CoreStack '
.' ( 
DefineResourcesAsync( <
(< =
	StackName= F
)F G
)G H
;H I
RoleAssignment 
roleAssignment %
=& '
result( .
.. /
	Resources/ 8
. 
OfType 
< 
RoleAssignment "
>" #
(# $
)$ %
. 
Single 
( 
x 
=> 
x 
. 
GetResourceName *
(* +
)+ ,
., -
Equals- 3
(3 4
$str4 N
,N O
StringComparisonP `
.` a
Ordinala h
)h i
)i j
;j k
string 
principalId 
= 
await "
OutputUtilities# 2
.2 3
GetValueAsync3 @
(@ A
roleAssignmentA O
.O P
PrincipalIdP [
)[ \
;\ ]
principalId 
. 
ShouldBe 
( 
$str C
)C D
;D E
} 
} ù
h/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Example.Tests.WithoutPulumock/StackOutputTests.cs
	namespace 	
Example
 
. 
Tests 
. 
WithoutPulumock '
;' (
public 
class 
StackOutputTests 
: 
TestBase  (
,( )
IStackOutputTests* ;
{ 
[ 
Fact 	
]	 

public 

async 
Task 0
$StackOutputs_ShouldOutputMockedValue :
(: ;
); <
{ 
( 	
ImmutableArray	 
< 
Resource  
>  !
	Resources" +
,+ ,
IDictionary- 8
<8 9
string9 ?
,? @
objectA G
?G H
>H I
StackOutputsJ V
)V W
resultX ^
=_ `
awaita f

Deploymentg q
.q r
	TestAsyncr {
({ |
new 
Mocks 
. 
Mocks 
( 
) 
, 
new 
TestOptions 
{ 
	IsPreview &
=' (
false) .
}. /
,/ 0
async 
( 
) 
=> 
await 
	CoreStack '
.' ( 
DefineResourcesAsync( <
(< =
	StackName= F
)F G
)G H
;H I
Vault 
keyVault 
= 
result 
.  
	Resources  )
. 
OfType 
< 
Vault 
> 
( 
) 
. 
Single 
( 
x 
=> 
x 
. 
GetResourceName *
(* +
)+ ,
., -
Equals- 3
(3 4
$str4 K
,K L
StringComparisonM ]
.] ^
Ordinal^ e
)e f
)f g
;g h#
VaultPropertiesResponse 
keyVaultProperties  2
=3 4
await5 :
OutputUtilities; J
.J K
GetValueAsyncK X
(X Y
keyVaultY a
.a b

Propertiesb l
)l m
;m n
string "
keyVaultUriStackOutput %
=& '
result( .
.. /
StackOutputs/ ;
[; <
$str< I
]I J
isK M
OutputN T
<T U
stringU [
>[ \
vaultUriOutput] k
? 
await 
OutputUtilities #
.# $
GetValueAsync$ 1
(1 2
vaultUriOutput2 @
)@ A
:   
throw   
new   %
InvalidOperationException   1
(  1 2
$str  2 Y
)  Y Z
;  Z ["
keyVaultUriStackOutput"" 
."" 
ShouldBe"" '
(""' (
keyVaultProperties""( :
."": ;
VaultUri""; C
)""C D
;""D E
}## 
}$$ ·i
h/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Example.Tests.WithoutPulumock/Shared/MocksBase.cs
	namespace 	
Example
 
. 
Tests 
. 
WithoutPulumock '
.' (
Shared( .
;. /
internal 
class	 
	MocksBase 
: 
IMocks !
{ 
public 

ImmutableList 
< 
ResourceSnapshot )
>) *
ResourceSnapshots+ <
=>= ?&
ProtectedResourceSnapshots@ Z
.Z [
ToImmutableList[ j
(j k
)k l
;l m
public		 

ImmutableList		 
<		 
CallSnapshot		 %
>		% &
CallSnapshots		' 4
=>		5 7"
ProtectedCallSnapshots		8 N
.		N O
ToImmutableList		O ^
(		^ _
)		_ `
;		` a
public 

virtual 
Task 
< 
( 
string 
?  
id! #
,# $
object% +
state, 1
)1 2
>2 3
NewResourceAsync4 D
(D E
MockResourceArgsE U
argsV Z
)Z [
{ 
ImmutableDictionary 
< 
string "
," #
object$ *
>* +
.+ ,
Builder, 3
outputs4 ;
=< =
ImmutableDictionary> Q
.Q R
CreateBuilderR _
<_ `
string` f
,f g
objecth n
>n o
(o p
)p q
;q r
outputs 
. 
AddRange 
( 
args 
. 
Inputs $
)$ %
;% &
if 

( 
string 
. 
Equals 
( 
args 
. 
Type #
,# $
$str% C
,C D
StringComparisonE U
.U V
OrdinalV ]
)] ^
)^ _
{ 	
var 
mockOutputs 
= 
new !

Dictionary" ,
<, -
string- 3
,3 4
object5 ;
>; <
(< =
)= >
;> ?
if 
( 
string 
. 
Equals 
( 
$"  
$str  6
{6 7
DevStackName7 C
}C D
"D E
,E F
argsG K
.K L
NameL P
,P Q
StringComparisonR b
.b c
Ordinalc j
)j k
|| 
string 
. 
Equals  
(  !
$"! #
$str# 9
{9 :
ProdStackName: G
}G H
"H I
,I J
argsK O
.O P
NameP T
,T U
StringComparisonV f
.f g
Ordinalg n
)n o
)o p
{ 
mockOutputs 
. 
Add 
(  
$str  H
,H I
$strJ p
)p q
;q r
} 
outputs 
. 
Add 
( 
$str !
,! "
mockOutputs# .
.. /!
ToImmutableDictionary/ D
(D E
)E F
)F G
;G H
outputs 
. 
Add 
( 
$str +
,+ ,
ImmutableArray- ;
<; <
string< B
>B C
.C D
EmptyD I
)I J
;J K
} 	
else 
{ 	
if   
(   
string   
.   
Equals   
(   
$str   D
,  D E
args  F J
.  J K
Type  K O
,  O P
StringComparison  Q a
.  a b
Ordinal  b i
)  i j
)  j k
{!! 
outputs"" 
."" 
Add"" 
("" 
$str"" -
,""- .
$str""/ ;
)""; <
;""< =
}## 
if%% 
(%% 
string%% 
.%% 
Equals%% 
(%% 
$str%% ;
,%%; <
args%%= A
.%%A B
Type%%B F
,%%F G
StringComparison%%H X
.%%X Y
Ordinal%%Y `
)%%` a
)%%a b
{&& 

Dictionary'' 
<'' 
string'' !
,''! "
object''# )
>'') *

properties''+ 5
=''6 7
outputs''8 ?
.''? @
TryGetValue''@ K
(''K L
$str''L X
,''X Y
out''Z ]
object''^ d
?''d e
value''f k
)''k l
&&''m o
value''p u
is''v x

Dictionary	''y É
<
''É Ñ
string
''Ñ ä
,
''ä ã
object
''å í
>
''í ì
existing
''î ú
?(( 
existing(( 
:)) 
new)) 

Dictionary)) $
<))$ %
string))% +
,))+ ,
object))- 3
>))3 4
())4 5
)))5 6
;))6 7

properties++ 
[++ 
$str++ %
]++% &
=++' (
$str++) J
;++J K
outputs,, 
[,, 
$str,, $
],,$ %
=,,& '

properties,,( 2
;,,2 3
}-- 
object//  
physicalResourceName// '
=//( )
outputs//* 1
.//1 2
GetValueOrDefault//2 C
(//C D
$str//D J
)//J K
??//L N
$"//O Q
{//Q R"
GetLogicalResourceName//R h
(//h i
args//i m
.//m n
Name//n r
)//r s
}//s t
$str//t }
"//} ~
;//~ 
outputs00 
.00 
Add00 
(00 
$str00 
,00  
physicalResourceName00  4
)004 5
;005 6
}11 	
string33 
resourceName33 
=33 "
GetLogicalResourceName33 4
(334 5
args335 9
.339 :
Name33: >
)33> ?
;33? @
string44 

resourceId44 
=44 
GetResourceId44 )
(44) *
args44* .
.44. /
Id44/ 1
,441 2
$"443 5
{445 6
resourceName446 B
}44B C
$str44C F
"44F G
)44G H
;44H I&
ProtectedResourceSnapshots66 "
.66" #
Add66# &
(66& '
new66' *
ResourceSnapshot66+ ;
(66; <
resourceName66< H
,66H I
args66J N
.66N O
Inputs66O U
)66U V
)66V W
;66W X
return88 
Task88 
.88 

FromResult88 
<88 
(88  
string88  &
?88& '
id88( *
,88* +
object88, 2
state883 8
)888 9
>889 :
(88: ;
(88; <

resourceId88< F
,88F G
outputs88H O
.88O P
ToImmutable88P [
(88[ \
)88\ ]
)88] ^
)88^ _
;88_ `
}99 
public;; 

virtual;; 
Task;; 
<;; 
object;; 
>;; 
	CallAsync;;  )
(;;) *
MockCallArgs;;* 6
args;;7 ;
);;; <
{<< 
ImmutableDictionary== 
<== 
string== "
,==" #
object==$ *
>==* +
.==+ ,
Builder==, 3
outputs==4 ;
===< =
ImmutableDictionary==> Q
.==Q R
CreateBuilder==R _
<==_ `
string==` f
,==f g
object==h n
>==n o
(==o p
)==p q
;==q r
outputs?? 
.?? 
AddRange?? 
(?? 
args?? 
.?? 
Args?? "
)??" #
;??# $
ifAA 

(AA 
stringAA 
.AA 
EqualsAA 
(AA 
GetCallTokenAA &
(AA& '
argsAA' +
.AA+ ,
TokenAA, 1
)AA1 2
,AA2 3
$strAA4 b
,AAb c
StringComparisonAAd t
.AAt u
OrdinalAAu |
)AA| }
)AA} ~
{BB 	
ifCC 
(CC 
argsCC 
.CC 
ArgsCC 
.CC 
TryGetValueCC %
(CC% &
$strCC& 8
,CC8 9
outCC: =
objectCC> D
?CCD E
valueCCF K
)CCK L
&&DD 
valueDD 
isDD 
stringDD "
existingDD# +
&&EE 
stringEE 
.EE 
EqualsEE  
(EE  !
existingEE! )
,EE) *
$strEE+ Q
,EEQ R
StringComparisonEES c
.EEc d
OrdinalEEd k
)EEk l
)EEl m
{FF 
outputsGG 
.GG 
AddGG 
(GG 
$strGG  
,GG  !
$strGG" H
)GGH I
;GGI J
}HH 
elseII 
{JJ 
outputsKK 
.KK 
AddKK 
(KK 
$strKK  
,KK  !
$strKK" H
)KKH I
;KKI J
}LL 
}MM 	
ImmutableDictionaryOO 
<OO 
stringOO "
,OO" #
objectOO$ *
>OO* +
finalOutputsOO, 8
=OO9 :
outputsOO; B
.OOB C
ToImmutableOOC N
(OON O
)OOO P
;OOP Q"
ProtectedCallSnapshotsQQ 
.QQ 
AddQQ "
(QQ" #
newQQ# &
CallSnapshotQQ' 3
(QQ3 4
GetCallTokenQQ4 @
(QQ@ A
argsQQA E
.QQE F
TokenQQF K
)QQK L
,QQL M
argsQQN R
.QQR S
ArgsQQS W
,QQW X
finalOutputsQQY e
)QQe f
)QQf g
;QQg h
returnSS 
TaskSS 
.SS 

FromResultSS 
<SS 
objectSS %
>SS% &
(SS& '
finalOutputsSS' 3
)SS3 4
;SS4 5
}TT 
publicVV 

constVV 
stringVV 
DevStackNameVV $
=VV% &
$strVV' ,
;VV, -
publicWW 

constWW 
stringWW 
ProdStackNameWW %
=WW& '
$strWW( .
;WW. /
	protectedYY 
readonlyYY 
ListYY 
<YY 
ResourceSnapshotYY ,
>YY, -&
ProtectedResourceSnapshotsYY. H
=YYI J
[YYK L
]YYL M
;YYM N
	protectedZZ 
readonlyZZ 
ListZZ 
<ZZ 
CallSnapshotZZ (
>ZZ( )"
ProtectedCallSnapshotsZZ* @
=ZZA B
[ZZC D
]ZZD E
;ZZE F
	protected\\ 
static\\ 
string\\ "
GetLogicalResourceName\\ 2
(\\2 3
string\\3 9
?\\9 :
name\\; ?
)\\? @
=>\\A C
string]] 
.]] 
IsNullOrWhiteSpace]] !
(]]! "
name]]" &
)]]& '
?]]( )
throw]]* /
new]]0 3!
ArgumentNullException]]4 I
(]]I J
nameof]]J P
(]]P Q
name]]Q U
)]]U V
)]]V W
:]]X Y
name]]Z ^
;]]^ _
	protected__ 
static__ 
string__ 
GetResourceId__ )
(__) *
string__* 0
?__0 1
id__2 4
,__4 5
string__6 <

fallbackId__= G
)__G H
=>__I K
string`` 
.`` 
IsNullOrWhiteSpace`` !
(``! "
id``" $
)``$ %
?``& '

fallbackId``( 2
:``3 4
id``5 7
;``7 8
	protectedbb 
staticbb 
stringbb 
GetCallTokenbb (
(bb( )
stringbb) /
?bb/ 0
tokenbb1 6
)bb6 7
=>bb8 :
stringcc 
.cc 
IsNullOrWhiteSpacecc !
(cc! "
tokencc" '
)cc' (
?cc) *
throwcc+ 0
newcc1 4!
ArgumentNullExceptioncc5 J
(ccJ K
nameofccK Q
(ccQ R
tokenccR W
)ccW X
)ccX Y
:ccZ [
tokencc\ a
;cca b
}dd 
internalff 
sealedff	 
recordff 
ResourceSnapshotff '
(ff' (
stringff( .
LogicalNameff/ :
,ff: ;
ImmutableDictionaryff< O
<ffO P
stringffP V
,ffV W
objectffX ^
>ff^ _
Inputsff` f
)fff g
;ffg h
internalhh 
sealedhh	 
recordhh 
CallSnapshothh #
(hh# $
stringhh$ *
Tokenhh+ 0
,hh0 1
ImmutableDictionaryhh2 E
<hhE F
stringhhF L
,hhL M
objecthhN T
>hhT U
InputshhV \
,hh\ ]
ImmutableDictionaryhh^ q
<hhq r
stringhhr x
,hhx y
object	hhz Ä
>
hhÄ Å
Outputs
hhÇ â
)
hhâ ä
;
hhä ã⁄e
e/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Example.Tests.WithoutPulumock/ResourceTests.cs
	namespace 	
Example
 
. 
Tests 
. 
WithoutPulumock '
;' (
public 
class 
ResourceTests 
: 
TestBase %
,% &
IResourceTests' 5
{ 
[ 
Fact 	
]	 

public 

async 
Task 
Resource_InputOnly (
(( )
)) *
{ 
var 
mocks 
= 
new 
Mocks 
. 
Mocks #
(# $
)$ %
;% &
_ 	
=
 
await 

Deployment 
. 
	TestAsync &
(& '
mocks 
, 
new 
TestOptions 
{ 
	IsPreview &
=' (
false) .
}. /
,/ 0
async 
( 
) 
=> 
await 
	CoreStack '
.' ( 
DefineResourcesAsync( <
(< =
	StackName= F
)F G
)G H
;H I
ResourceSnapshot 
resourceSnapshot )
=* +
mocks, 1
.1 2
ResourceSnapshots2 C
.C D
SingleD J
(J K
xK L
=>M O
xP Q
.Q R
LogicalNameR ]
.] ^
Equals^ d
(d e
$stre v
,v w
StringComparison	x à
.
à â
Ordinal
â ê
)
ê ë
)
ë í
;
í ì
if 

( 
! 
resourceSnapshot 
. 
Inputs $
.$ %
TryGetValue% 0
(0 1
$str1 D
,D E
outF I
objectJ P
?P Q
valueR W
)W X
||Y [
value\ a
isb d
note h
stringi o
resourceGroupName	p Å
)
Å Ç
{ 	
throw 
new  
KeyNotFoundException *
(* +
$str+ w
)w x
;x y
} 	
resourceGroupName 
. 
ShouldBe "
(" #
$str# 4
)4 5
;5 6
}   
["" 
Fact"" 	
]""	 

public## 

async## 
Task## 
Resource_OutputOnly## )
(##) *
)##* +
{$$ 
(%% 	
ImmutableArray%%	 
<%% 
Resource%%  
>%%  !
	Resources%%" +
,%%+ ,
IDictionary%%- 8
<%%8 9
string%%9 ?
,%%? @
object%%A G
?%%G H
>%%H I
StackOutputs%%J V
)%%V W
result%%X ^
=%%_ `
await%%a f

Deployment%%g q
.%%q r
	TestAsync%%r {
(%%{ |
new&& 
Mocks&& 
.&& 
Mocks&& 
(&& 
)&& 
,&& 
new'' 
TestOptions'' 
{'' 
	IsPreview'' &
=''' (
false'') .
}''. /
,''/ 0
async(( 
((( 
)(( 
=>(( 
await(( 
	CoreStack(( '
.((' ( 
DefineResourcesAsync((( <
(((< =
	StackName((= F
)((F G
)((G H
;((H I
ResourceGroup** 
resourceGroup** #
=**$ %
result**& ,
.**, -
	Resources**- 6
.++ 
OfType++ 
<++ 
ResourceGroup++ !
>++! "
(++" #
)++# $
.,, 
Single,, 
(,, 
x,, 
=>,, 
x,, 
.,, 
GetResourceName,, *
(,,* +
),,+ ,
.,,, -
Equals,,- 3
(,,3 4
$str,,4 E
,,,E F
StringComparison,,G W
.,,W X
Ordinal,,X _
),,_ `
),,` a
;,,a b
string.. 
azureApiVersion.. 
=..  
await..! &
OutputUtilities..' 6
...6 7
GetValueAsync..7 D
(..D E
resourceGroup..E R
...R S
AzureApiVersion..S b
)..b c
;..c d
azureApiVersion// 
.// 
ShouldBe//  
(//  !
$str//! -
)//- .
;//. /
}00 
[22 
Fact22 	
]22	 

public33 

async33 
Task33  
Resource_InputOutput33 *
(33* +
)33+ ,
{44 
var55 
mocks55 
=55 
new55 
Mocks55 
.55 
Mocks55 #
(55# $
)55$ %
;55% &
(66 	
ImmutableArray66	 
<66 
Resource66  
>66  !
	Resources66" +
,66+ ,
IDictionary66- 8
<668 9
string669 ?
,66? @
object66A G
?66G H
>66H I
StackOutputs66J V
)66V W
result66X ^
=66_ `
await66a f

Deployment66g q
.66q r
	TestAsync66r {
(66{ |
mocks77 
,77 
new88 
TestOptions88 
{88 
	IsPreview88 &
=88' (
false88) .
}88. /
,88/ 0
async99 
(99 
)99 
=>99 
await99 
	CoreStack99 '
.99' ( 
DefineResourcesAsync99( <
(99< =
	StackName99= F
)99F G
)99G H
;99H I
ResourceGroup;; 
resourceGroup;; #
=;;$ %
result;;& ,
.;;, -
	Resources;;- 6
.<< 
OfType<< 
<<< 
ResourceGroup<< !
><<! "
(<<" #
)<<# $
.== 
Single== 
(== 
x== 
=>== 
x== 
.== 
GetResourceName== *
(==* +
)==+ ,
.==, -
Equals==- 3
(==3 4
$str==4 E
,==E F
StringComparison==G W
.==W X
Ordinal==X _
)==_ `
)==` a
;==a b
ResourceSnapshot?? 
resourceSnapshot?? )
=??* +
mocks??, 1
.??1 2
ResourceSnapshots??2 C
.??C D
Single??D J
(??J K
x??K L
=>??M O
x??P Q
.??Q R
LogicalName??R ]
.??] ^
Equals??^ d
(??d e
$str??e v
,??v w
StringComparison	??x à
.
??à â
Ordinal
??â ê
)
??ê ë
)
??ë í
;
??í ì
if@@ 

(@@ 
!@@ 
resourceSnapshot@@ 
.@@ 
Inputs@@ $
.@@$ %
TryGetValue@@% 0
(@@0 1
$str@@1 ;
,@@; <
out@@= @
object@@A G
?@@G H
value@@I N
)@@N O
||@@P R
value@@S X
is@@Y [
not@@\ _
string@@` f
locationFromInput@@g x
)@@x y
{AA 	
throwBB 
newBB  
KeyNotFoundExceptionBB *
(BB* +
$strBB+ n
)BBn o
;BBo p
}CC 	
stringEE 
locationFromOutputEE !
=EE" #
awaitEE$ )
OutputUtilitiesEE* 9
.EE9 :
GetValueAsyncEE: G
(EEG H
resourceGroupEEH U
.EEU V
LocationEEV ^
)EE^ _
;EE_ `
locationFromInputGG 
.GG 
ShouldBeGG "
(GG" #
$strGG# 2
)GG2 3
;GG3 4
locationFromOutputHH 
.HH 
ShouldBeHH #
(HH# $
$strHH$ 3
)HH3 4
;HH4 5
}II 
[KK 
FactKK 	
]KK	 

publicLL 

asyncLL 
TaskLL 
Resource_DependencyLL )
(LL) *
)LL* +
{MM 
varNN 
mocksNN 
=NN 
newNN 
MocksNN 
.NN 
MocksNN #
(NN# $
)NN$ %
;NN% &
(OO 	
ImmutableArrayOO	 
<OO 
ResourceOO  
>OO  !
	ResourcesOO" +
,OO+ ,
IDictionaryOO- 8
<OO8 9
stringOO9 ?
,OO? @
objectOOA G
?OOG H
>OOH I
StackOutputsOOJ V
)OOV W
resultOOX ^
=OO_ `
awaitOOa f

DeploymentOOg q
.OOq r
	TestAsyncOOr {
(OO{ |
mocksPP 
,PP 
newQQ 
TestOptionsQQ 
{QQ 
	IsPreviewQQ &
=QQ' (
falseQQ) .
}QQ. /
,QQ/ 0
asyncRR 
(RR 
)RR 
=>RR 
awaitRR 
	CoreStackRR '
.RR' ( 
DefineResourcesAsyncRR( <
(RR< =
	StackNameRR= F
)RRF G
)RRG H
;RRH I
ResourceGroupTT 
resourceGroupTT #
=TT$ %
resultTT& ,
.TT, -
	ResourcesTT- 6
.UU 
OfTypeUU 
<UU 
ResourceGroupUU !
>UU! "
(UU" #
)UU# $
.VV 
SingleVV 
(VV 
xVV 
=>VV 
xVV 
.VV 
GetResourceNameVV *
(VV* +
)VV+ ,
.VV, -
EqualsVV- 3
(VV3 4
$strVV4 E
,VVE F
StringComparisonVVG W
.VVW X
OrdinalVVX _
)VV_ `
)VV` a
;VVa b
ResourceSnapshotXX 
resourceSnapshotXX )
=XX* +
mocksXX, 1
.XX1 2
ResourceSnapshotsXX2 C
.XXC D
SingleXXD J
(XXJ K
xXXK L
=>XXM O
xXXP Q
.XXQ R
LogicalNameXXR ]
.XX] ^
EqualsXX^ d
(XXd e
$strXXe |
,XX| }
StringComparison	XX~ é
.
XXé è
Ordinal
XXè ñ
)
XXñ ó
)
XXó ò
;
XXò ô
ifYY 

(YY 
!YY 
resourceSnapshotYY 
.YY 
InputsYY $
.YY$ %
TryGetValueYY% 0
(YY0 1
$strYY1 D
,YYD E
outYYF I
objectYYJ P
?YYP Q
valueYYR W
)YYW X
||YYY [
valueYY\ a
isYYb d
notYYe h
stringYYi o
resourceGroupName	YYp Å
)
YYÅ Ç
{ZZ 	
throw[[ 
new[[  
KeyNotFoundException[[ *
([[* +
$str[[+ w
)[[w x
;[[x y
}\\ 	
resourceGroupName^^ 
.^^ 
ShouldBe^^ "
(^^" #
await^^# (
OutputUtilities^^) 8
.^^8 9
GetValueAsync^^9 F
(^^F G
resourceGroup^^G T
.^^T U
Name^^U Y
)^^Y Z
)^^Z [
;^^[ \
}__ 
[aa 
Factaa 	
]aa	 

publicbb 

asyncbb 
Taskbb 
Resource_Multiplebb '
(bb' (
)bb( )
{cc 
(dd 	
ImmutableArraydd	 
<dd 
Resourcedd  
>dd  !
	Resourcesdd" +
,dd+ ,
IDictionarydd- 8
<dd8 9
stringdd9 ?
,dd? @
objectddA G
?ddG H
>ddH I
StackOutputsddJ V
)ddV W
resultddX ^
=dd_ `
awaitdda f

Deploymentddg q
.ddq r
	TestAsyncddr {
(dd{ |
newee 
Mocksee 
.ee 
Mocksee 
(ee 
)ee 
,ee 
newff 
TestOptionsff 
{ff 
	IsPreviewff &
=ff' (
falseff) .
}ff. /
,ff/ 0
asyncgg 
(gg 
)gg 
=>gg 
awaitgg 
	CoreStackgg '
.gg' ( 
DefineResourcesAsyncgg( <
(gg< =
	StackNamegg= F
)ggF G
)ggG H
;ggH I
IEnumerableii 
<ii 
Resourceii 
>ii 
	resourcesii '
=ii( )
resultii* 0
.ii0 1
	Resourcesii1 :
.jj 
OfTypejj 
<jj 
Resourcejj 
>jj 
(jj 
)jj 
;jj  
	resourcesll 
.ll 
ShouldAllBell 
(ll 
xll 
=>ll  "
!ll# $
stringll$ *
.ll* +
IsNullOrWhiteSpacell+ =
(ll= >
xll> ?
.ll? @
GetResourceNamell@ O
(llO P
)llP Q
)llQ R
)llR S
;llS T
}mm 
}nn ë
c/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Example.Tests.WithoutPulumock/Mocks/Mocks.cs
	namespace 	
Example
 
. 
Tests 
. 
WithoutPulumock '
.' (
Mocks( -
;- .
internal 
sealed	 
class 
Mocks 
: 
	MocksBase '
;' (f
d/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Example.Tests.WithoutPulumock/GlobalUsings.cs˚)
j/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Example.Tests.WithoutPulumock/ConfigurationTests.cs
	namespace 	
Example
 
. 
Tests 
. 
WithoutPulumock '
;' (
public 
sealed 
class 
ConfigurationTests &
:' (
TestBase) 1
,1 2
IConfigurationTests3 F
{ 
[ 
Fact 	
]	 

public 

async 
Task 0
$Config_MockedConfigurationInResource :
(: ;
); <
{ 
var 
mocks 
= 
new 
Mocks 
. 
Mocks #
(# $
)$ %
;% &
_ 	
=
 
await 

Deployment 
. 
	TestAsync &
(& '
mocks 
, 
new 
TestOptions 
{ 
	IsPreview &
=' (
false) .
}. /
,/ 0
async 
( 
) 
=> 
await 
	CoreStack '
.' ( 
DefineResourcesAsync( <
(< =
	StackName= F
)F G
)G H
;H I
ResourceSnapshot 
resourceSnapshot )
=* +
mocks, 1
.1 2
ResourceSnapshots2 C
.C D
SingleD J
(J K
xK L
=>M O
xP Q
.Q R
LogicalNameR ]
.] ^
Equals^ d
(d e
$stre |
,| }
StringComparison	~ é
.
é è
Ordinal
è ñ
)
ñ ó
)
ó ò
;
ò ô
if 

( 
! 
resourceSnapshot 
. 
Inputs $
.$ %
TryGetValue% 0
(0 1
$str1 =
,= >
out? B
objectC I
?I J
propertiesObjK X
)X Y
||Z \
propertiesObj 
is 
not  
IDictionary! ,
<, -
string- 3
,3 4
object5 ;
>; <

properties= G
)G H
{ 	
throw 
new  
KeyNotFoundException *
(* +
$str+ p
)p q
;q r
} 	
if   

(   
!   

properties   
.   
TryGetValue   #
(  # $
$str  $ .
,  . /
out  0 3
object  4 :
?  : ;
tenantIdObj  < G
)  G H
||  I K
tenantIdObj!! 
is!! 
not!! 
string!! %
tenantId!!& .
)!!. /
{"" 	
throw## 
new##  
KeyNotFoundException## *
(##* +
$str##+ n
)##n o
;##o p
}$$ 	
tenantId&& 
.&& 
ShouldBe&& 
(&& 
$str&& @
)&&@ A
;&&A B
}'' 
[)) 
Fact)) 	
]))	 

public** 

async** 
Task** )
Config_MockedSecretInResource** 3
(**3 4
)**4 5
{++ 
(,, 	
ImmutableArray,,	 
<,, 
Resource,,  
>,,  !
	Resources,," +
,,,+ ,
IDictionary,,- 8
<,,8 9
string,,9 ?
,,,? @
object,,A G
?,,G H
>,,H I
StackOutputs,,J V
),,V W
result,,X ^
=,,_ `
await,,a f

Deployment,,g q
.,,q r
	TestAsync,,r {
(,,{ |
new-- 
Mocks-- 
.-- 
Mocks-- 
(-- 
)-- 
,-- 
new.. 
TestOptions.. 
{.. 
	IsPreview.. &
=..' (
false..) .
}... /
,../ 0
async// 
(// 
)// 
=>// 
await// 
	CoreStack// '
.//' ( 
DefineResourcesAsync//( <
(//< =
	StackName//= F
)//F G
)//G H
;//H I
Secret11 
secret11 
=11 
result11 
.11 
	Resources11 (
.22 
OfType22 
<22 
Secret22 
>22 
(22 
)22 
.33 
Single33 
(33 
x33 
=>33 
x33 
.33 
GetResourceName33 *
(33* +
)33+ ,
.33, -
Equals33- 3
(333 4
$str334 g
,33g h
StringComparison33i y
.33y z
Ordinal	33z Å
)
33Å Ç
)
33Ç É
;
33É Ñ$
SecretPropertiesResponse55  
secretProperties55! 1
=552 3
await554 9
OutputUtilities55: I
.55I J
GetValueAsync55J W
(55W X
secret55X ^
.55^ _

Properties55_ i
)55i j
;55j k
secretProperties66 
.66 
Value66 
.66 
ShouldBe66 '
(66' (
$str66( ;
)66; <
;66< =
}77 
}88 Éj
n/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Example.Tests.WithoutPulumock/ComponentResourceTests.cs
	namespace 	
Example
 
. 
Tests 
. 
WithoutPulumock '
;' (
public 
class "
ComponentResourceTests #
:$ %
TestBase& .
,. /#
IComponentResourceTests0 G
{ 
[ 
Fact 	
]	 

public 

async 
Task 
ComponentResource '
(' (
)( )
{ 
var 
mocks 
= 
new 
Mocks 
. 
Mocks #
(# $
)$ %
;% &
( 	
ImmutableArray	 
< 
Resource  
>  !
	Resources" +
,+ ,
IDictionary- 8
<8 9
string9 ?
,? @
objectA G
?G H
>H I
OutputsJ Q
)Q R
resultS Y
=Z [
await\ a

Deploymentb l
.l m
	TestAsyncm v
(v w
mocks 
, 
new 
TestOptions 
{ 
	IsPreview &
=' (
false) .
}. /
,/ 0
( 
) 
=> 
{ 
var 
componentResource %
=& '
new( +0
$KeyVaultWithSecretsComponentResource, P
(P Q
$strQ b
,b c
newd g
(g h
)h i
{ 
	VaultName 
= 
$str  7
,7 8
ResourceGroupName %
=& '
$str( 9
,9 :
TenantId 
= 
$str E
,E F
Secrets 
= 
new !
(! "
)" #
{ 
{   
$str   5
,  5 6
$str  7 J
}  J K
}!! 
}"" 
)"" 
;"" 
return$$ 
new$$ 

Dictionary$$ %
<$$% &
string$$& ,
,$$, -
object$$. 4
?$$4 5
>$$5 6
{%% 
{&& 
$str&&  
,&&  !
componentResource&&" 3
.&&3 4
KeyVault&&4 <
}&&= >
}'' 
;'' 
}(( 
)(( 
;(( 0
$KeyVaultWithSecretsComponentResource** ,
componentResource**- >
=**? @
result**A G
.**G H
	Resources**H Q
.++ 
OfType++ 
<++ 0
$KeyVaultWithSecretsComponentResource++ 8
>++8 9
(++9 :
)++: ;
.,, 
Single,, 
(,, 
),, 
;,, 
Vault.. 
keyVault.. 
=.. 
result.. 
...  
	Resources..  )
.// 
OfType// 
<// 
Vault// 
>// 
(// 
)// 
.00 
Single00 
(00 
)00 
;00 
Secret22 
secret22 
=22 
result22 
.22 
	Resources22 (
.33 
OfType33 
<33 
Secret33 
>33 
(33 
)33 
.44 
Single44 
(44 
)44 
;44 $
SecretPropertiesResponse66  
secretProperties66! 1
=662 3
await664 9
OutputUtilities66: I
.66I J
GetValueAsync66J W
(66W X
secret66X ^
.66^ _

Properties66_ i
)66i j
;66j k
componentResource88 
.88 
GetResourceName88 )
(88) *
)88* +
.88+ ,
ShouldBe88, 4
(884 5
$str885 F
)88F G
;88G H
keyVault99 
.99 
GetResourceName99  
(99  !
)99! "
.99" #
ShouldBe99# +
(99+ ,
$str99, C
)99C D
;99D E
secret:: 
.:: 
GetResourceName:: 
(:: 
)::  
.::  !
ShouldBe::! )
(::) *
$str::* ]
)::] ^
;::^ _
secretProperties;; 
.;; 
Value;; 
.;; 
ShouldBe;; '
(;;' (
$str;;( ;
);;; <
;;;< =
}<< 
[>> 
Fact>> 	
]>>	 

public?? 

async?? 
Task?? ;
/ComponentResource_MissingNonRequiredResourceArg?? E
(??E F
)??F G
{@@ 
varAA 
mocksAA 
=AA 
newAA 
MocksAA 
.AA 
MocksAA #
(AA# $
)AA$ %
;AA% &
(BB 	
ImmutableArrayBB	 
<BB 
ResourceBB  
>BB  !
	ResourcesBB" +
,BB+ ,
IDictionaryBB- 8
<BB8 9
stringBB9 ?
,BB? @
objectBBA G
?BBG H
>BBH I
OutputsBBJ Q
)BBQ R
resultBBS Y
=BBZ [
awaitBB\ a

DeploymentBBb l
.BBl m
	TestAsyncBBm v
(BBv w
mocksCC 
,CC 
newDD 
TestOptionsDD 
{DD 
	IsPreviewDD &
=DD' (
falseDD) .
}DD. /
,DD/ 0
(EE 
)EE 
=>EE 
{FF 
varGG 
componentResourceGG %
=GG& '
newGG( +0
$KeyVaultWithSecretsComponentResourceGG, P
(GGP Q
$strGGQ b
,GGb c
newGGd g
(GGg h
)GGh i
{HH 
	VaultNameII 
=II 
$strII  7
,II7 8
ResourceGroupNameJJ %
=JJ& '
$strJJ( 9
,JJ9 :
TenantIdKK 
=KK 
$strKK E
}LL 
)LL 
;LL 
returnNN 
newNN 

DictionaryNN %
<NN% &
stringNN& ,
,NN, -
objectNN. 4
?NN4 5
>NN5 6
{OO 
{PP 
$strPP  
,PP  !
componentResourcePP" 3
.PP3 4
KeyVaultPP4 <
}PP= >
}QQ 
;QQ 
}RR 
)RR 
;RR 
SecretTT 
?TT 
secretTT 
=TT 
resultTT 
.TT  
	ResourcesTT  )
.UU 
OfTypeUU 
<UU 
SecretUU 
>UU 
(UU 
)UU 
.VV 
SingleOrDefaultVV 
(VV 
)VV 
;VV 
secretXX 
.XX 
ShouldBeNullXX 
(XX 
)XX 
;XX 
}YY 
[[[ 
Fact[[ 	
][[	 

public\\ 

async\\ 
Task\\ $
ComponentResource_Parent\\ .
(\\. /
)\\/ 0
{]] 
var^^ 
mocks^^ 
=^^ 
new^^ 
Mocks^^ 
.^^ 
Mocks^^ #
(^^# $
)^^$ %
;^^% &
(__ 	
ImmutableArray__	 
<__ 
Resource__  
>__  !
	Resources__" +
,__+ ,
IDictionary__- 8
<__8 9
string__9 ?
,__? @
object__A G
?__G H
>__H I
Outputs__J Q
)__Q R
result__S Y
=__Z [
await__\ a

Deployment__b l
.__l m
	TestAsync__m v
(__v w
mocks`` 
,`` 
newaa 
TestOptionsaa 
{aa 
	IsPreviewaa &
=aa' (
falseaa) .
}aa. /
,aa/ 0
(bb 
)bb 
=>bb 
{cc 
vardd 
componentResourcedd %
=dd& '
newdd( +0
$KeyVaultWithSecretsComponentResourcedd, P
(ddP Q
$strddQ b
,ddb c
newddd g
(ddg h
)ddh i
{ee 
	VaultNameff 
=ff 
$strff  7
,ff7 8
ResourceGroupNamegg %
=gg& '
$strgg( 9
,gg9 :
TenantIdhh 
=hh 
$strhh E
,hhE F
Secretsii 
=ii 
newii !
(ii! "
)ii" #
{jj 
{kk 
$strkk 5
,kk5 6
$strkk7 J
}kkJ K
}ll 
}mm 
)mm 
;mm 
returnoo 
newoo 

Dictionaryoo %
<oo% &
stringoo& ,
,oo, -
objectoo. 4
?oo4 5
>oo5 6
{pp 
{qq 
$strqq  
,qq  !
componentResourceqq" 3
.qq3 4
KeyVaultqq4 <
}qq= >
}rr 
;rr 
}ss 
)ss 
;ss 0
$KeyVaultWithSecretsComponentResourceuu ,
componentResourceuu- >
=uu? @
resultuuA G
.uuG H
	ResourcesuuH Q
.vv 
OfTypevv 
<vv 0
$KeyVaultWithSecretsComponentResourcevv 8
>vv8 9
(vv9 :
)vv: ;
.ww 
Singleww 
(ww 
)ww 
;ww 
Vaultyy 
keyVaultyy 
=yy 
resultyy 
.yy  
	Resourcesyy  )
.zz 
OfTypezz 
<zz 
Vaultzz 
>zz 
(zz 
)zz 
.{{ 
Single{{ 
({{ 
){{ 
;{{ 
Secret}} 
secret}} 
=}} 
result}} 
.}} 
	Resources}} (
.~~ 
OfType~~ 
<~~ 
Secret~~ 
>~~ 
(~~ 
)~~ 
. 
Single 
( 
) 
; 
TestHelpers
ÅÅ 
.
ÅÅ 
	IsChildOf
ÅÅ 
(
ÅÅ 
keyVault
ÅÅ &
,
ÅÅ& '
componentResource
ÅÅ( 9
)
ÅÅ9 :
.
ÅÅ: ;
ShouldBeTrue
ÅÅ; G
(
ÅÅG H
)
ÅÅH I
;
ÅÅI J
TestHelpers
ÇÇ 
.
ÇÇ 
	IsChildOf
ÇÇ 
(
ÇÇ 
secret
ÇÇ $
,
ÇÇ$ %
componentResource
ÇÇ& 7
)
ÇÇ7 8
.
ÇÇ8 9
ShouldBeTrue
ÇÇ9 E
(
ÇÇE F
)
ÇÇF G
;
ÇÇG H
}
ÉÉ 
[
ÖÖ 
Fact
ÖÖ 	
]
ÖÖ	 

public
ÜÜ 

async
ÜÜ 
Task
ÜÜ '
ComponentResource_Outputs
ÜÜ /
(
ÜÜ/ 0
)
ÜÜ0 1
{
áá 
var
àà 
mocks
àà 
=
àà 
new
àà 
Mocks
àà 
.
àà 
Mocks
àà #
(
àà# $
)
àà$ %
;
àà% &
(
ââ 	
ImmutableArray
ââ	 
<
ââ 
Resource
ââ  
>
ââ  !
	Resources
ââ" +
,
ââ+ ,
IDictionary
ââ- 8
<
ââ8 9
string
ââ9 ?
,
ââ? @
object
ââA G
?
ââG H
>
ââH I
Outputs
ââJ Q
)
ââQ R
result
ââS Y
=
ââZ [
await
ââ\ a

Deployment
ââb l
.
ââl m
	TestAsync
ââm v
(
ââv w
mocks
ää 
,
ää 
new
ãã 
TestOptions
ãã 
{
ãã 
	IsPreview
ãã &
=
ãã' (
false
ãã) .
}
ãã. /
,
ãã/ 0
(
åå 
)
åå 
=>
åå 
{
çç 
var
éé 
componentResource
éé %
=
éé& '
new
éé( +2
$KeyVaultWithSecretsComponentResource
éé, P
(
ééP Q
$str
ééQ b
,
ééb c
new
ééd g
(
éég h
)
ééh i
{
èè 
	VaultName
êê 
=
êê 
$str
êê  7
,
êê7 8
ResourceGroupName
ëë %
=
ëë& '
$str
ëë( 9
,
ëë9 :
TenantId
íí 
=
íí 
$str
íí E
}
ìì 
)
ìì 
;
ìì 
return
ïï 
new
ïï 

Dictionary
ïï %
<
ïï% &
string
ïï& ,
,
ïï, -
object
ïï. 4
?
ïï4 5
>
ïï5 6
{
ññ 
{
óó 
$str
óó  
,
óó  !
componentResource
óó" 3
.
óó3 4
KeyVault
óó4 <
}
óó= >
}
òò 
;
òò 
}
ôô 
)
ôô 
;
ôô 
if
õõ 

(
õõ 
!
õõ 
result
õõ 
.
õõ 
Outputs
õõ 
.
õõ 
TryGetValue
õõ '
(
õõ' (
$str
õõ( 2
,
õõ2 3
out
õõ4 7
object
õõ8 >
?
õõ> ?
keyVaultObj
õõ@ K
)
õõK L
||
õõM O
keyVaultObj
õõP [
is
õõ\ ^
not
õõ_ b
Vault
õõc h
keyVault
õõi q
)
õõq r
{
úú 	
throw
ùù 
new
ùù "
KeyNotFoundException
ùù *
(
ùù* +
$str
ùù+ n
)
ùùn o
;
ùùo p
}
ûû 	
keyVault
†† 
.
†† 
GetResourceName
††  
(
††  !
)
††! "
.
††" #
ShouldBe
††# +
(
††+ ,
$str
††, C
)
††C D
;
††D E
}
°° 
}¢¢ ∑@
a/Users/robertopiranamedi/RiderProjects/Pulumock/Source/Example.Tests.WithoutPulumock/CallTests.cs
	namespace 	
Example
 
. 
Tests 
. 
WithoutPulumock '
;' (
public 
class 
	CallTests 
: 
TestBase !
,! "

ICallTests# -
{ 
[ 
Fact 	
]	 

public 

async 
Task 

Call_Input  
(  !
)! "
{ 
var 
mocks 
= 
new 
Mocks 
. 
Mocks #
(# $
)$ %
;% &
_ 	
=
 
await 

Deployment 
. 
	TestAsync &
(& '
mocks 
, 
new 
TestOptions 
{ 
	IsPreview &
=' (
false) .
}. /
,/ 0
async 
( 
) 
=> 
await 
	CoreStack '
.' ( 
DefineResourcesAsync( <
(< =
	StackName= F
)F G
)G H
;H I
var 
calls 
= 
mocks 
. 
CallSnapshots '
. 
Where 
( 
x 
=> 
x 
. 
Token 
.  
Equals  &
(& '
$str' U
,U V
StringComparisonW g
.g h
Ordinalh o
)o p
)p q
. 
ToList 
( 
) 
; 
var 
roleDefinitionIds 
= 
calls  %
. 
Select 
( 
call 
=> 
call  
.  !
Inputs! '
.' (
TryGetValue( 3
(3 4
$str4 F
,F G
outH K
objectL R
?R S
roleDefinitionIdT d
)d e
?f g
roleDefinitionIdh x
asy {
string	| Ç
:
É Ñ
null
Ö â
)
â ä
. 
Where 
( 
id 
=> 
id 
is 
not "
null# '
)' (
. 
ToList 
( 
) 
; 
calls!! 
.!! 
Count!! 
.!! 
ShouldBe!! 
(!! 
$num!! 
)!! 
;!!  
roleDefinitionIds"" 
."" 
ShouldContain"" '
(""' (
$str""( N
)""N O
;""O P
roleDefinitionIds## 
.## 
ShouldContain## '
(##' (
$str##( N
)##N O
;##O P
}$$ 
[&& 
Fact&& 	
]&&	 

public'' 

async'' 
Task'' 
Call_Output'' !
(''! "
)''" #
{(( 
var)) 
mocks)) 
=)) 
new)) 
Mocks)) 
.)) 
Mocks)) #
())# $
)))$ %
;))% &
_** 	
=**
 
await** 

Deployment** 
.** 
	TestAsync** &
(**& '
mocks++ 
,++ 
new,, 
TestOptions,, 
{,, 
	IsPreview,, &
=,,' (
false,,) .
},,. /
,,,/ 0
async-- 
(-- 
)-- 
=>-- 
await-- 
	CoreStack-- '
.--' ( 
DefineResourcesAsync--( <
(--< =
	StackName--= F
)--F G
)--G H
;--H I
var// 
calls// 
=// 
mocks// 
.// 
CallSnapshots// '
.00 
Where00 
(00 
x00 
=>00 
x11 
.11 
Token11 
.11 
Equals11 
(11 
$str11 M
,11M N
StringComparison11O _
.11_ `
Ordinal11` g
)11g h
&&22 
x22 
.22 
Inputs22 
.22 
TryGetValue22 '
(22' (
$str22( :
,22: ;
out22< ?
object22@ F
?22F G
roleDefinitionId22H X
)22X Y
&&33 
roleDefinitionId33 #
is33$ &
string33' -
id33. 0
&&44 
id44 
.44 
Equals44 
(44 
$str44 C
,44C D
StringComparison44E U
.44U V
Ordinal44V ]
)44] ^
)44^ _
.55 
ToList55 
(55 
)55 
;55 
var77  
getRoleDefinitionIds77  
=77! "
calls77# (
.88 
Select88 
(88 
call88 
=>88 
call88  
.88  !
Outputs88! (
.88( )
TryGetValue88) 4
(884 5
$str885 9
,889 :
out88; >
object88? E
?88E F
id88G I
)88I J
?99 
id99 
as99 
string99 
::: 
throw:: 
new:: %
InvalidOperationException:: 5
(::5 6
$str::6 R
)::R S
)::S T
.;; 
ToList;; 
(;; 
);; 
;;;  
getRoleDefinitionIds== 
.== 
ShouldAllBe== (
(==( )
id==) +
=>==, .
string==/ 5
.==5 6
Equals==6 <
(==< =
id=== ?
,==? @
$str==A g
,==g h
StringComparison==i y
.==y z
Ordinal	==z Å
)
==Å Ç
)
==Ç É
;
==É Ñ
}>> 
[@@ 
Fact@@ 	
]@@	 

publicAA 

asyncAA 
TaskAA #
Call_ResourceDependencyAA -
(AA- .
)AA. /
{BB 
varCC 
mocksCC 
=CC 
newCC 
MocksCC 
.CC 
MocksCC #
(CC# $
)CC$ %
;CC% &
(DD 	
ImmutableArrayDD	 
<DD 
ResourceDD  
>DD  !
	ResourcesDD" +
,DD+ ,
IDictionaryDD- 8
<DD8 9
stringDD9 ?
,DD? @
objectDDA G
?DDG H
>DDH I
StackOutputsDDJ V
)DDV W
resultDDX ^
=DD_ `
awaitDDa f

DeploymentDDg q
.DDq r
	TestAsyncDDr {
(DD{ |
mocksEE 
,EE 
newFF 
TestOptionsFF 
{FF 
	IsPreviewFF &
=FF' (
falseFF) .
}FF. /
,FF/ 0
asyncGG 
(GG 
)GG 
=>GG 
awaitGG 
	CoreStackGG '
.GG' ( 
DefineResourcesAsyncGG( <
(GG< =
	StackNameGG= F
)GGF G
)GGG H
;GGH I
RoleAssignmentII 
roleAssignmentII %
=II& '
resultII( .
.II. /
	ResourcesII/ 8
.JJ 
OfTypeJJ 
<JJ 
RoleAssignmentJJ "
>JJ" #
(JJ# $
)JJ$ %
.KK 
SingleKK 
(KK 
xKK 
=>KK 
xKK 
.KK 
GetResourceNameKK *
(KK* +
)KK+ ,
.KK, -
EqualsKK- 3
(KK3 4
$strKK4 N
,KKN O
StringComparisonKKP `
.KK` a
OrdinalKKa h
)KKh i
)KKi j
;KKj k
stringMM 
roleDefinitionIdMM 
=MM  !
awaitMM" '
OutputUtilitiesMM( 7
.MM7 8
GetValueAsyncMM8 E
(MME F
roleAssignmentMMF T
.MMT U
RoleDefinitionIdMMU e
)MMe f
;MMf g
roleDefinitionIdNN 
.NN 
ShouldBeNN !
(NN! "
$strNN" H
)NNH I
;NNI J
}OO 
}PP 