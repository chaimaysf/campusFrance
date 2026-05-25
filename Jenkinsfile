pipeline {
    agent any

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
                sh '/opt/homebrew/bin/dotnet test'
            }
        }
    }
}