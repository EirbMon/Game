FROM ubuntu:16.04
WORKDIR /Game
COPY . /Game
RUN chmod 777 BuildLinuxServer/Eirbmon.x86_64 
CMD ["./BuildLinuxServer/Eirbmon.x86_64"]