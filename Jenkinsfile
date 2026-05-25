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
            sh '/opt/homebrew/bin/allure generate bin/Debug/net10.0/allure-results --clean -o allure-report'
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
        success {
            emailext(
                to: 'chaimaysf2000@gmail.com',
                subject: "✅ Build SUCCESS - campusFrance #${BUILD_NUMBER}",
                body: """
                    <h2>Build réussi ✅</h2>
                    <p>Job : ${JOB_NAME}</p>
                    <p>Build : #${BUILD_NUMBER}</p>
                    <p>Environnement : ${params.ENV}</p>
                    <p>Rapport Allure : <a href="${BUILD_URL}HTML_20Report">Voir le rapport</a></p>
                """,
                mimeType: 'text/html'
            )
        }
        failure {
            emailext(
                to: 'chaimaysf2000@gmail.com',
                subject: "❌ Build FAILED - campusFrance #${BUILD_NUMBER}",
                body: """
                    <h2>Build échoué ❌</h2>
                    <p>Job : ${JOB_NAME}</p>
                    <p>Build : #${BUILD_NUMBER}</p>
                    <p>Environnement : ${params.ENV}</p>
                    <p>Logs : <a href="${BUILD_URL}console">Voir les logs</a></p>
                """,
                mimeType: 'text/html'
            )
        }
    }
}