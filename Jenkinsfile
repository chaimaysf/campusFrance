pipeline {
    agent any
    parameters {
        choice(name: 'ENV', choices: ['test', 'pp'], description: 'Environnement cible')
    }
    stages {
        stage('Checkout') { steps { checkout scm } }
        stage('Build') { steps { sh '/opt/homebrew/bin/dotnet build' } }
        stage('Test') { steps {
            echo "Exécution sur l'environnement : ${params.ENV}"
            sh '/opt/homebrew/bin/dotnet test'
        }}
        stage('Report') { steps {
            sh '/opt/homebrew/bin/allure generate allure-results --clean -o allure-report'
        }}
    }
    post {
        always {
            publishHTML(target: [
                allowMissing: false,
                alwaysLinkToLastBuild: true,
                keepAll: true,
                reportDir: 'allure-report',
                reportFiles: 'index.html',
                reportName: 'Allure Report'
            ])
        }
    }
}