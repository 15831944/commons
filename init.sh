#bash -c "$(wget https://raw.githubusercontent.com/crystalsis/commons/master/init.sh -O -)"
#su - root
usermod -a -G sudo jakaz
usermod -a -G adm jakaz
apt-get install -y sudo vim zsh fish git curl wget busybox htop iotop iftop unzip liblxc1 strace fonts-powerline lrzsz cmatrix cmatrix-xfont boxes libaa-bin sl toilet figlet bb fortune-mod fortune-zh cowsay lolcat screenfetch linuxlogo neofetch aview imagemagick pv espeak rig bastet ninvaders pacman4console nsnake greed bsdgames bsdgames moon-buggy pi ri fonts-noto fonts-ipafont-mincho fonts-ipafont-gothic fonts-arphic-ukai fonts-arphic-uming fonts-nanum fonts-mona fonts-vlgothic fonts-takao-gothic fonts-ipafont-gothic fonts-ipaexfont-gothic fonts-takao-mincho fonts-ipafont-mincho fonts-ipaexfont-mincho
sh -c "$(curl -fsSL https://raw.githubusercontent.com/robbyrussell/oh-my-zsh/master/tools/install.sh)"
curl -L https://get.oh-my.fish | fish
omf install lambda fonts mvn rvm
wget https://releases.hashicorp.com/consul/1.4.0/consul_1.4.0_linux_amd64.zip
unzip consul_1.4.0_linux_amd64.zip
chmod +x consul
mv consul /usr/bin/consul
rm -f consul_1.4.0_linux_amd64.zip
wget https://releases.hashicorp.com/vault/1.0.0/vault_1.0.0_linux_amd64.zip
unzip vault_1.0.0_linux_amd64.zip
chmod +x vault
mv vault /usr/bin/vault
rm -f vault_1.0.0_linux_amd64.zip
wget https://releases.hashicorp.com/nomad/0.8.6/nomad_0.8.6_linux_amd64-lxc.zip
unzip nomad_0.8.6_linux_amd64-lxc.zip
chmod +x nomad
mv nomad /usr/bin/nomad
rm -f nomad_0.8.6_linux_amd64-lxc.zip
wget https://github.com/fabiolb/fabio/releases/download/v1.5.10/fabio-1.5.10-go1.11.1-linux_amd64
chmod +x fabio-1.5.10-go1.11.1-linux_amd64
mv fabio-1.5.10-go1.11.1-linux_amd64 /usr/bin/fabio
rm -f fabio-1.5.10-go1.11.1-linux_amd64
