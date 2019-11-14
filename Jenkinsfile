pipeline {
  agent any
  stages {  
    stage('Build') {
      parallel {
        stage('Build dev') {
          when {
            branch "dev"
          }
          steps {
            sh 'docker build -t eirbmon/game-dev .'
            echo 'Docker dev image built'
          }
        }
        stage('Build prod') {
          when {
            branch "master"
          }
          steps {
            sh 'docker build -t eirbmon/game .'
            echo 'Docker prod image built'
          }
        }
        stage('Stop old dev') {
          when {
            branch "dev"
          }
          steps {
            sh 'cp -r BuildInfo/* /home/eirbmon/Documents/SharedUnity/dev'
            sh 'docker stop eirbmon-game-dev || true'
            sh 'docker rm eirbmon-game-dev || true'
            sh 'docker rmi eirbmon/game-dev || true'
            echo 'Old dev container stopped'
          }
        }
        stage('Stop old prod') {
          when {
            branch "master"
          }
          steps {
            // sh 'cp -r BuildInfo/* /home/eirbmon/Documents/SharedUnity/prod'
            sh 'cp -r BuildInfo/* /home/atia/Desktop/SharedUnity/prod'
            sh 'docker stop eirbmon-game || true'
            sh 'docker rm eirbmon-game || true'
            sh 'docker rmi eirbmon/game || true'
            echo 'Old prod container stopped'
          }
        }
      }
    }
    stage('Run dev container') {
      when {
        branch "dev"
      }
      steps {
        sh 'docker run -p 6666:7777 -it -v /home/eirbmon/Documents/SharedUnity/dev:/Game/BuildInfo -d --name eirbmon-game-dev eirbmon/game-dev'
        echo 'Dev container ready !'
      }
    }
    stage('Run prod container') {
      when {
        branch "master"
      }
      steps {
        sh 'docker run -p 7777:7777 -it -v /home/eirbmon/Documents/SharedUnity/prod:/Game/BuildInfo -d --name eirbmon-game eirbmon/game'
        echo 'Prod container ready !'
      }
    }
  }  
}