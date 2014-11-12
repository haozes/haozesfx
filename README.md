HaozesFx(China Mobile Fetion tool)
========
A project about China Mobile Fetion,written by C#, which implements Fetion SIPC protocol. 

The project shows how to login and send/receive Fetion message,add friend or accept friend request.


The old source host:
http://haozesfx.codeplex.com/

The project written by haozes at 2009,so it may not run propely. Let me know if you want to use it.

##  Description
There are three parts in project:  

1. FxClient:  Fetion login process and SIPC Protocol parser.  

2. FxRobot:	 The host of FxClient show how to use in a winform application,which can forwards received msg to fxplugin.  

3. FxPlugin: plugins can reiceve Fetion messages then parse the text to execute some cmds like fetch weather report,send/reiceve e-mail,etc.


## Get started
1. Download the source
2. edit FxRobot/app.config with your phone and fetion password(ChinaMobile)

       <!--移动手机号-->
        <add key="Telephone" value="xxxxxx" />
        <!--飞信登陆密码-->
        <add key="Password" value="xxxxx"/>
    
3. Run build.cmd to build
4. Run FxRobot.exe.

##Update History

update:
##### 2014/11/12
move to github

##### 2011/01/04:
1.修正由于新协议中使用了chat server,导致收信慢的问题

##### 2010/10/14 :

1.增加了验证码情况的处理.
2.天气预报插件中的:
Too many automatic redirections were attempted 的错误
ps:解决方案转换为vs2010了,请注意

##### 2010/09/16:
已升级到2010协议.登陆有验证码情况,未处理.好友管理操作待验证,更新.

##### 2010/09/11:
近日飞信协议升级,2008的协议以无法使用,正在更新中...