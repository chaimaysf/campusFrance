pipeline {
    agent any

    parameters {
        choice(name: 'ENV', choices: ['test', 'pp'], description: 'Environnement cible')
    }

    stages {
        stage('Checkout') {
            steps {
                checkout scm
            }
        }

        stage('Build') {
            steps {
                sh '/opt/homebrew/bin/dotnet build'
            }
        }

        stage('Test') {
            steps {
                echo "Exécution sur l'environnement : ${params.ENV}"
                sh '/opt/homebrew/bin/dotnet test'
            }
        }
    }
}


