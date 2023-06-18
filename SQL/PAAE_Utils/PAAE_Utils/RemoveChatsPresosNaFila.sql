
--Localizar os ids: http://10.50.5.47:5799/?funcao=estatistica
--Remover as interações presas nos dois servidores http://localhost:5799/?funcao=removeworkbin&ixnid=00018aJ04EU9704G

SELECT *
FROM Alctel_Gen_IXN..interactions
WHERE
	media_type = 'chat' AND [State] = 2
	AND Id IN (
		'00018aJ04EU992KE'
        ,'00018aJ04EU992W7'
        ,'00018aJ04EU993DH'
        ,'00018aJ04EU993DU'
        ,'00018aJ04EU993N0'
        ,'00018aJ04EU995C2'
        ,'00018aJ04EU995XY'
        ,'00018aJ04EU995YC'
        ,'00018aJ04EU996S6'
        ,'00018aJ04EU997DS'
        ,'00018aJ04EU9986M'
        ,'00018aJ04EU998F0'
        ,'00018aJ04EU998PX'
        ,'00018aJ04EU998Y3'
        ,'00018aJ04EU9992F'
        ,'00018aJ04EU999CR'
        ,'00018aJ04EU999RN'
        ,'00018aJ04EU99AU1'
        ,'00018aJ04EU99BKU'
        ,'00018aJ04EU99C9T'
        ,'00018aJ04EU99CFG'
        ,'00018aJ04EU99D5G'
        ,'00018aJ04EU99D8T'
        ,'00018aJ04EU99D9J'
        ,'00018aJ04EU99E98'
        ,'00018aJ04EU99EJD'
        ,'00018aJ04EU99FB3'
        ,'00018aJ04EU99G4W'
        ,'00018aJ04EU99H8J'
        ,'00018aJ04EU99HCJ'
        ,'00018aJ04EU99HXA'
        ,'00018aJ04EU99J0V'
        ,'00018aJ04EU99JBJ'
        ,'00018aJ04EU99JKP'
        ,'00018aJ04EU99K68'
        ,'00018aJ04EU99KH9')
	ORDER BY received_at