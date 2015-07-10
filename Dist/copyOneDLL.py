#! /usr/bin/env python

import os,shutil,sys 

prefix = 'BerwickHeights.Platform.';
os.chdir('..');
filename = prefix + sys.argv[1];
dirname = filename + '/bin/Debug/';
print 'Copying files from ' + dirname;
shutil.copyfile(dirname + filename + '.dll', 'Dist/' + filename + '.dll');
shutil.copyfile(dirname + filename + '.pdb', 'Dist/' + filename + '.pdb');
shutil.copyfile(dirname + filename + '.xml', 'Dist/' + filename + '.xml');

