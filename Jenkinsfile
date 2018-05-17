node('master') {
    stage('import') {
        try {
            git branch: 'develop', url: 'https://github.com/1804-Apr-USFdotnet/Project2-QuadCore-Bookmarker.git'
        }
        catch (exc) {
            slackError('import')
            throw exc
        }
    }
    stage('build') {
        try {
            dir('Bookmarker.MVC') {
                bat 'nuget restore'
                bat 'msbuild /p:MvcBuildViews=true'
            }
        }
        catch (exc) {
            slackError('build')
            throw exc
        }
    }
    stage('test') {
        try {
            //dir('Bookmarker.MVC') {
                //bat "VSTest.Console *test*\\bin\\Debug\\*Test.dll"
            //}
        }
        catch (exc) {
            slackError('test')
            throw exc
        }
    }
    stage('analyze') {
        try {
            dir('Bookmarker.MVC') {
                bat 'SonarScanner.MSBuild begin /k:bookmarker-project2 /v:0.1.0 /d:sonar.verbose=true'
                bat 'msbuild /t:rebuild'
                bat 'SonarScanner.MSBuild end'
            }
        }
        catch (exc) {
            slackError('analyze')
            throw exc
        }
    }
    stage('package') {
        try {
            dir('Bookmarker.MVC') {
                bat 'msbuild /t:package /p:Configuration=Debug;PackageFileName=..\\Package.zip'
            }
        }
        catch (exc) {
            slackError('package')
            throw exc
        }
    }
    stage('deploy') {
        try {
            dir('Bookmarker.MVC') {
                bat "msdeploy -verb:sync -source:package=\"%CD%\\Package.zip\" -dest:auto,computerName=\"ec2-18-205-198-149.compute-1.amazonaws.com\",userName=\"Administrator\",password=\"${env.Deploy_Password}\",authtype=\"basic\",includeAcls=\"False\" -disableLink:AppPoolExtension -disableLink:ContentExtension -disableLink:CertificateExtension -setParam:\"IIS Web Application Name\"=\"Default Web Site/Bookmarker\" -enableRule:AppOffline -allowUntrusted"
            }
        }
        catch (exc) {
            slackError('deploy')
            throw exc
        }
    }
}

def slackError(stageName) {
    slackSend color: 'danger', message: "FAILED ${stageName} stage: [<${JOB_URL}|${env.JOB_NAME}> <${env.BUILD_URL}console|${env.BUILD_DISPLAY_NAME}>] [${currentBuild.durationString.replace(' and counting', '')}]"
}
