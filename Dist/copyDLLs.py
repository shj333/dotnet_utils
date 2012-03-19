#! /usr/bin/env python

import os,shutil    

skippedDirs = '';
os.chdir('..');
for filename in os.listdir('.') :
    if not filename.startswith('BerwickHeights') :
        skippedDirs += 'Skipped ' + filename + '\n';
        continue;
    if filename.endswith('.Test') :
        skippedDirs += 'Skipped ' + filename + '\n';
        continue;
        
    print 'Copying ' + filename;    
    dirname = filename + '/bin/Debug/';
    shutil.copyfile(dirname + filename + '.dll', 'Dist/' + filename + '.dll');
    shutil.copyfile(dirname + filename + '.pdb', 'Dist/' + filename + '.pdb');
    shutil.copyfile(dirname + filename + '.xml', 'Dist/' + filename + '.xml');


print '\n\n' + skippedDirs;

