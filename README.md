# shop
***
## 使用redis 这里只记录windows的使用方式

1. https://github.com/microsoftarchive/redis/tags 下载最新的版本
2. 修改配置文件redis.windows.conf（1.注释bind 127.0.0.1 2.修改保护模式protected-mode yes为protected-mode no）记得要开通端口（包括云服务器）

## cmd 打开redis命令 在redis文件夹内打开cmd
1. 打开serve: redis-server redis.windows.conf
2. 打开client: redis-cli.exe -h 127.0.0.1 -a 123456 -p 6379 (-h:ip地址 -a 密码 -p 端口)
